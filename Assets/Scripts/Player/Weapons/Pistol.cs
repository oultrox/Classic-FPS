using System;
using System.Collections;
using UnityEngine;

public class Pistol : MonoBehaviour {

    [Header("Animations")]
    [SerializeField] private Sprite idlePistol;
    [SerializeField] private Sprite shotPistol;

    [Header("Damage")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float range = 100;
    [SerializeField] private GameObject bulletHolePrefab;

    [Header("Ammunation")]
    [SerializeField] private int ammoAmount = 200;
    [SerializeField] private int ammoClipSize = 12;
    [SerializeField] private float reloadTime = 1;
    [SerializeField] private float shakeDuration = 0.15f;

    private SpriteRenderer spriteRenderer;
    private Vector3 firePosition;
    private int ammoLeft;
    private int ammoClipLeft;
    private bool isReloading = false;
    
    // ------------------------------------------------------
    // API Methods
    // ------------------------------------------------------

    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        ammoClipLeft = ammoClipSize;
        ammoLeft = ammoAmount;
        firePosition = new Vector3(Screen.width/2, Screen.height/2, 0);
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && isReloading == false)
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R)  && isReloading == false)
        {
            Reload();
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

        StartCoroutine(AnimateShot());
        ammoClipLeft -= 1;
        
        Ray ray = Camera.main.ScreenPointToRay(firePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.gameObject.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<Enemy>().TakeDamage(damage);
            }
            Instantiate(bulletHolePrefab,hit.point,Quaternion.FromToRotation(Vector3.up,hit.normal)).transform.parent = hit.transform;
        }
        DynamicCrosshair.instance.ExpansionTimer = 0.02f;
        CameraShake.shakeDuration = shakeDuration;
    }

    private void Reload()
    {
        StartCoroutine(ReloadWeapon());
    }
    
    private IEnumerator AnimateShot()
    {
        spriteRenderer.sprite = shotPistol;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = idlePistol;
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
            else if(ammoLeft > 0 && ammoLeft < bulletsToReload)
            {
                ammoClipLeft += ammoLeft;
                ammoLeft = 0;
            }
        }
    }
}
