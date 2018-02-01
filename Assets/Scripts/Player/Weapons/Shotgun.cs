using System.Collections;
using UnityEngine;

public class Shotgun : MonoBehaviour {

    [Header("Damage")]
    [SerializeField]
    private int damage = 20;

    [Header("Fire stuff")]
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float maxFireDistance = 100f;
    [SerializeField] private float fireRadius = 10f;
    [SerializeField] private LayerMask shootLayer;

    [Header("Ammunation")]
    [SerializeField] private int ammoAmount = 200;
    [SerializeField] private int ammoClipSize = 12;
    [SerializeField] private float reloadTime = 1;

    [Header("Shake")]
    [SerializeField] private float shakeDuration = 0.08f;
    [SerializeField] private float shakeMagnitude = 4;

    private Transform camTransform;
    private Animator anim;
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
        shootCooldown = fireRate;
        camTransform = Camera.main.transform;
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
    private RaycastHit hit;

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

        if (Physics.SphereCast(camTransform.position, fireRadius, camTransform.forward, out hit, maxFireDistance, shootLayer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
        }
        DynamicCrosshair.instance.ExpansionTimer = 0.03f;
        CameraShake.instance.StartShakeRotating(shakeDuration, shakeMagnitude);
        WeaponShake.instance.StartShake(shakeDuration, 0.1f);

        //Check after in order to reload automatic if there's enough projectiles.
        if (ammoClipLeft <= 0)
        {
            Reload();
            return;
        }
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

    private void OnDrawGizmos()
    {
        if (hit.collider)
        {
            Gizmos.DrawRay(camTransform.position, camTransform.forward * hit.distance);
            Gizmos.DrawWireSphere(transform.position + transform.forward * (hit.distance), fireRadius );
        }
    }

}
