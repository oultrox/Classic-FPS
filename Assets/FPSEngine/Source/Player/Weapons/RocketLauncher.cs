using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using DumbInjector;
using UnityEngine;

public class RocketLauncher : Weapon {

    [Header("Prefab referencies")]
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject spawnPoint;

    [Header("Rocket Properties")]
    [SerializeField] private float rocketForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] LayerMask explosionLayer;

    [Inject] CameraShaker playerCameraShaker;
    [Inject] WeaponShake _weaponShaker;
    [Inject] DynamicCrosshair _crosshair;
    private bool isCharged;
    private bool isShot;

    // ------------------------------------------------------
    // API Methods
    // ------------------------------------------------------

    private void Awake()
    {
        AmmoClipLeft = AmmoClipSize;
        AmmoLeft = AmmoAmount;
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

        if (Input.GetKeyDown(KeyCode.R) && IsReloading == false)
        {
            Reload();
        }
    }

    private void OnEnable()
    {
        this.spawnPoint.SetActive(true);
    }

    private void OnDisable()
    {
        this.spawnPoint.SetActive(false);
    }

    // ------------------------------------------------------
    // Custom methods
    // ------------------------------------------------------

    // If anything goes wrong just put this function in FixedUpdate() and add an variable that conects to the input in Update().
    private void Shoot()
    {
        if (AmmoClipLeft <= 0)
        {
            Reload();
            return;
        }

        AmmoClipLeft -= 1;
        GameObject rocketInstantiated = Instantiate(rocketPrefab, spawnPoint.transform.position, Quaternion.identity);
        rocketInstantiated.GetComponent<Rocket>().Init(Damage,explosionRadius,explosionLayer,explosionPrefab);
        Rigidbody rocketRbody = rocketInstantiated.GetComponent<Rigidbody>();
        rocketRbody.AddForce(Camera.main.transform.forward * rocketForce, ForceMode.Impulse);

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
}
