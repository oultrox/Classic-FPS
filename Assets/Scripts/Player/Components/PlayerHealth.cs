﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	[SerializeField] private int startingHealth;
	private static int currentHealth;
    private static bool isDead;
	
	// Use this for initialization
	void Start () 
	{
		currentHealth = startingHealth;
		isDead = false;
	}
	
    public void TakeDamage(int damage)
    {
		Debug.Log("me daño! " + currentHealth);
        currentHealth -= damage;
        //Si tiene menor que 0 su vida murió
        if (currentHealth <= 0)
        {
			
            Dead();
        }
    }

    private void Dead()
    {
        this.GetComponent<PlayerMovement>().enabled = false;
        this.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}