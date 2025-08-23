using System;
using System.Collections;
using DumbInjector;
using UnityEngine;

public class Pistol : Weapon {

    [Header("Animations")]
    [SerializeField] private Sprite idlePistol;
    [SerializeField] private Sprite shotPistol;
    [SerializeField] private float range = 100;
    [SerializeField] private GameObject bulletHolePrefab;
    [Inject] CameraShaker playerCameraShaker;
    [Inject] WeaponShake _weaponShaker;
    [Inject] DynamicCrosshair _crosshair;
    
    private SpriteRenderer spriteRenderer;
    private Vector3 firePosition;

    
    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        AmmoClipLeft = AmmoClipSize;
        AmmoLeft = AmmoAmount;
        firePosition = new Vector3(Screen.width/2, Screen.height/2, 0);
    }

    private void Update()
    {
        HandleShooting();
    }

    private void HandleShooting()
    {
        if (Input.GetButtonDown("Fire1") && IsReloading == false)
        {
            Shoot();
        }

        if (Input.GetKeyDown(KeyCode.R)  && IsReloading == false)
        {
            Reload();
        }
    }
    
    // If anything goes wrong just put this function in FixedUpdate() and add an variable that conects to the input in Update().
    private void Shoot()
    {
        if (AmmoClipLeft <= 0)
        {
            Reload();
            return;
        }

        StartCoroutine(AnimateShot());
        AmmoClipLeft -= 1;
        
        Ray ray = Camera.main.ScreenPointToRay(firePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<IHasHealth>().TakeDamage(Damage);
            }
            if (!hit.collider.CompareTag("Projectile"))
            {
                Instantiate(bulletHolePrefab, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal)).transform.parent = hit.transform;
            }
        }
        _crosshair.ExpansionTimer = 0.02f;
        playerCameraShaker.StartShakeRotating(ShakeDuration, ShakeMagnitude);
        _weaponShaker.StartShake(ShakeDuration, 0.1f);

        //Check after in order to reload automatic if there's enough projectiles.
        if (AmmoClipLeft <= 0)
        {
            Reload();
            return;
        }
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
        int bulletsToReload = AmmoClipSize - AmmoClipLeft;
        // If there's something to reload - activate coroutine for the delay. 
        if (bulletsToReload > 0 && AmmoLeft > 0)
        {
            IsReloading = true;
            yield return new WaitForSeconds(ReloadTime);
        }
        else
        {
            //no ammo.
        }

        if (IsReloading)
        {
            IsReloading = false;
            if (AmmoLeft >= bulletsToReload)
            {
                AmmoLeft -= bulletsToReload;
                AmmoClipLeft = AmmoClipSize;
            }
            else if(AmmoLeft > 0 && AmmoLeft < bulletsToReload)
            {
                AmmoClipLeft += AmmoLeft;
                AmmoLeft = 0;
            }
        }
    }
}
