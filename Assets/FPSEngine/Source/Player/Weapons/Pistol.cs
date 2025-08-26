using System.Collections;
using DumbInjector;
using FPSEngine.Source.DI.Containers;
using UnityEngine;

public class Pistol : Weapon
{
    const float RAYCAST_RANGE = 100;
    Sprite idlePistol;
    Sprite shotPistol;
    GameObject bulletHolePrefab;
    SpriteRenderer spriteRenderer;
    Vector3 firePosition;
    
    
    [Inject]
    public void InjectContainer(WeaponContainer weaponContainer)
    {
        idlePistol = weaponContainer.IdlePistol;
        shotPistol = weaponContainer.ShotPistol;
        bulletHolePrefab = weaponContainer.BulletHolePrefab;
    }
    
    private void Awake()
    {
        InitBehaviors();
    }
    
    private void Update()
    {
        HandleShooting();
    }
    
    void InitBehaviors()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        AmmoClipLeft = AmmoClipSize;
        AmmoLeft = AmmoAmount;
        firePosition = new Vector3(Screen.width/2, Screen.height/2, 0);
    }

    void HandleShooting()
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
    void Shoot()
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
        if (Physics.Raycast(ray, out hit, RAYCAST_RANGE))
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
        _cameraShaker.StartShakeRotating(ShakeDuration, ShakeMagnitude);
        _weaponShaker.StartShake(ShakeDuration, 0.1f);

        //Check after in order to reload automatic if there's enough projectiles.
        if (AmmoClipLeft <= 0)
        {
            Reload();
        }
    }

    void Reload()
    {
        StartCoroutine(ReloadWeapon());
    }
    
    IEnumerator AnimateShot()
    {
        spriteRenderer.sprite = shotPistol;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = idlePistol;
    }

    IEnumerator ReloadWeapon()
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
