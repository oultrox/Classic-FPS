using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour {

    [SerializeField] private float fireRate = 1f;

    [Header("Damage")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float range = 100;

    [Header("Ammunation")]
    [SerializeField] private int ammoAmount = 200;
    [SerializeField] private int ammoClipSize = 12;
    [SerializeField] private float reloadTime = 1;

    [Header("Shake")]
    [SerializeField] private float shakeDuration = 0.08f;
    [SerializeField] private float shakeMagnitude = 4;

    private Animator anim;
    private Vector3 firePosition;
    private int ammoLeft;
    private int ammoClipLeft;
    private bool isReloading = false;
    private float shootCooldown = 0f;
    // ------------------------------------------------------
    // API Methods
    // ------------------------------------------------------

    private void Awake()
    {
        anim = GetComponent<Animator>();
        ammoClipLeft = ammoClipSize;
        ammoLeft = ammoAmount;
        firePosition = new Vector3(Screen.width / 2, Screen.height / 2, 0);
        shootCooldown = fireRate;
    }

    private void Update()
    {
        if (ManagerScreen.instance.IsPaused())
        {
            return;
        }

        if (shootCooldown <= fireRate)
        {
            shootCooldown += Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
        {
            Reload();
        }
        
        if (Input.GetButtonDown("Fire1") && isReloading == false && shootCooldown >= fireRate)
        {
            Shoot();
        }
    }

    // ------------------------------------------------------
    // Custom methods
    // ------------------------------------------------------
    // If anything goes wrong just put this function in FixedUpdate() and add an variable that conects to the input in Update().
    private void Shoot()
    {
        if (ammoClipLeft <= 0)
        {
            Reload();
            return;
        }
        shootCooldown = 0f;
        anim.SetTrigger("Shoot");
        ammoClipLeft -= 1;

        Ray ray = Camera.main.ScreenPointToRay(firePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        DynamicCrosshair.instance.ExpansionTimer = 0.03f;
        CameraShake.instance.StartShakeRotating(shakeDuration, shakeMagnitude);
        WeaponShake.instance.StartShake(shakeDuration, 0.1f);
    }

    private void Reload()
    {
        StartCoroutine(ReloadWeapon());
    }

    private IEnumerator ReloadWeapon()
    {
        int bulletsToReload = ammoClipSize - ammoClipLeft;
        // If there's something to reload - activate coroutine for the delay. 
        if (bulletsToReload > 0 && ammoLeft > 0)
        {
            isReloading = true;
            yield return new WaitForSeconds(reloadTime);
        }
        else
        {
            //no ammo.
        }

        if (isReloading)
        {
            isReloading = false;
            if (ammoLeft >= bulletsToReload)
            {
                ammoLeft -= bulletsToReload;
                ammoClipLeft = ammoClipSize;
            }
            else if (ammoLeft > 0 && ammoLeft < bulletsToReload)
            {
                ammoClipLeft += ammoLeft;
                ammoLeft = 0;
            }
        }
    }

}
