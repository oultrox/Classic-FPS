using System.Collections;
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
        
        currentHealth -= damage;
        //Si tiene menor que 0 su vida murió
        if (currentHealth <= 0)
        {
            Dead();
        }
        CameraShake.instance.StartShakeRotating(0.09f, 5f);
        ManagerGUI.instance.HurtBlink();
    }

    private void Dead()
    {
        isDead = true;
        this.GetComponent<PlayerMovement>().enabled = false;
        this.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}
