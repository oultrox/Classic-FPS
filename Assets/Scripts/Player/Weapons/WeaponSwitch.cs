using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitch : MonoBehaviour {

    [SerializeField] private List<Transform> weapons;
    [SerializeField] private int initialWeapon;
    private int selectedWeapon;

	void Start ()
    {
        selectedWeapon = initialWeapon % weapons.Count;
        UpdateWeapon();
	}

    void Update ()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            selectedWeapon = (selectedWeapon + 1) % weapons.Count;

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedWeapon > 0)
            {
                selectedWeapon = selectedWeapon - 1 % weapons.Count;
            }
            else
            {
                selectedWeapon = weapons.Count - 1;
            }
        }
           

        if (Input.GetKeyDown(KeyCode.Alpha1))
            selectedWeapon = 0;

        if (Input.GetKeyDown(KeyCode.Alpha2))
            selectedWeapon = 1;

        if (Input.GetKeyDown(KeyCode.Alpha3))
            selectedWeapon = 2;


        UpdateWeapon();
    }

    private void UpdateWeapon()
    {
        for (int i = 0; i < weapons.Count; i++)
        {
            if (i == selectedWeapon)
            {
                weapons[i].gameObject.SetActive(true);
            }
            else
            {
                weapons[i].gameObject.SetActive(false);
            }
        }
    }

}
