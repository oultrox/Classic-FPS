using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pistol : MonoBehaviour {

    [Header("Animations")]
    [SerializeField] private Sprite idlePistol;
    [SerializeField] private Sprite shotPistol;

    [Header("Damage")]
    [SerializeField] private float damage;
    [SerializeField] private float range;

    [Header("Ammunation")]
    [SerializeField] private int ammoAmount;
    [SerializeField] private int ammoClipSize;

    private SpriteRenderer spriteRenderer;
    private int ammoLeft;
    private int ammoClipLeft;

    // ------------------------------------------------------
    // API Methods
    // ------------------------------------------------------
    private void Awake()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        ammoClipLeft = ammoClipSize;
        ammoLeft = ammoAmount;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            ShootRay();
            
        }
    }

    // ------------------------------------------------------
    // Custom methods
    // ------------------------------------------------------
    private IEnumerator AnimateShot()
    {
        spriteRenderer.sprite = shotPistol;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.sprite = idlePistol;
    }

    //If anything goes wrong just put this function in FixedUpdate() and add an variable that conects to the input in Update().
    private void ShootRay()
    {
        if (ammoClipLeft <= 0)
        {
            Reload();
            return;
        }

        StartCoroutine(AnimateShot());
        ammoClipLeft -= 1;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, range))
        {
            Debug.Log("Pew pew!" + hit.collider.gameObject.name);
            hit.collider.gameObject.SendMessage("PistolHit", damage, SendMessageOptions.DontRequireReceiver);
        }
    }

    private void Reload()
    {
        int bulletsToReload = ammoClipSize - ammoClipLeft;
        if (ammoLeft > bulletsToReload)
        {
            ammoLeft -= bulletsToReload;
            ammoClipLeft = bulletsToReload;
        }
        else if(ammoLeft > 0 && ammoLeft < bulletsToReload)
        {
            ammoClipLeft = ammoLeft;
            ammoLeft = 0;
        }
        else if (ammoLeft <= 0)
        {
            Debug.Log("Can't reload!");
        }
    }
}
