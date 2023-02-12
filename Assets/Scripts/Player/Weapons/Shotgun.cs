using System.Collections;
using UnityEngine;

public class Shotgun : Weapon {


    [Header("Shotgun stuff")]
    [SerializeField] private float fireRate = 1f;
    [SerializeField] private float maxFireDistance = 100f;
    [SerializeField] private float fireRadius = 10f;
    [SerializeField] private LayerMask shootLayer;

    private Transform camTransform;
    private Animator anim;
    private float shootCooldown = 0f;
    private RaycastHit hit;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        AmmoClipLeft = AmmoClipSize;
        AmmoLeft = AmmoAmount;
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

        if (Input.GetKeyDown(KeyCode.R) && IsReloading == false)
        {
            Reload();
        }
        
        if (Input.GetButtonDown("Fire1") && IsReloading == false && shootCooldown >= fireRate)
        {
            Shoot();
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

        shootCooldown = 0f;
        anim.SetTrigger("Shoot");
        AmmoClipLeft -= 1;

        if (Physics.SphereCast(camTransform.position, fireRadius, camTransform.forward, out hit, maxFireDistance, shootLayer))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                hit.collider.GetComponent<EnemyController>().TakeDamage(Damage);
            }
        }
        DynamicCrosshair.instance.ExpansionTimer = 0.03f;
        ManagerShake.instance.StartShakeRotating(ShakeDuration, ShakeMagnitude);
        ManagerShake.instance.StartZoomEffect();
        WeaponShake.instance.StartShake(ShakeDuration, 0.1f);
        
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
            else if (AmmoLeft > 0 && AmmoLeft < bulletsToReload)
            {
                AmmoClipLeft += AmmoLeft;
                AmmoLeft = 0;
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
