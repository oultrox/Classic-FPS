using System;
using System.Collections;
using System.Collections.Generic;
using DumbInjector;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHasHealth
{
	[SerializeField] private int startingHealth;
	private int currentHealth;
    private static bool isDead;
    
	void Start () 
	{
		currentHealth = startingHealth;
		isDead = false;
	}

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0 || isDead)
        {
            Dead();
        }
        CameraShaker.instance.StartShakeRotating(0.09f, 5f);
        ManagerGUI.instance.HurtBlink();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void Dead()
    {
        isDead = true;
        this.GetComponent<PlayerMovement>().enabled = false;
        this.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}
