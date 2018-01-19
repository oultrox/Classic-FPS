using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : MonoBehaviour {

    [Header("Prefab referencies")]
    [SerializeField] private GameObject rocketPrefab;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private GameObject spawnPoint;

    [Header("Bullet & damage")]
    [SerializeField] private int damage = 20;
    [SerializeField] private float rocketForce;
    [SerializeField] private float explosionRadius;
    [SerializeField] LayerMask explosionLayer;

    [Header("Ammunation")]
    [SerializeField] private int ammoAmount = 200;
    [SerializeField] private int ammoClipSize = 12;
    [SerializeField] private float reloadTime = 1;

    [Header("Shake")]
    [SerializeField] private float shakeDuration = 0.08f;
    [SerializeField] private float shakeMagnitude = 4;

    private int ammoLeft;
    private int ammoClipLeft;
    private bool isReloading;
    private bool isCharged;
    private bool isShot;

    // ------------------------------------------------------
    // API Methods
    // ------------------------------------------------------

    private void Awake()
    {
        ammoClipLeft = ammoClipSize;
        ammoLeft = ammoAmount;
    }

    private void Update()
    {
        if (ManagerScreen.instance.IsPaused())
        {
            return;
        }

        if (Input.GetButtonDown("Fire1") && isReloading == false)
        {
            Shoot();
        }
        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
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
        if (ammoClipLeft <= 0)
        {
            Reload();
            return;
        }

        GameObject rocketInstantiated = Instantiate(rocketPrefab, spawnPoint.transform.position, Quaternion.identity);
        rocketInstantiated.GetComponent<Rocket>().Init(damage,explosionRadius,explosionLayer,explosionPrefab);
        Rigidbody rocketRbody = rocketInstantiated.GetComponent<Rigidbody>();
        rocketRbody.AddForce(Camera.main.transform.forward * rocketForce, ForceMode.Impulse);

        DynamicCrosshair.instance.ExpansionTimer = 0.02f;
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
