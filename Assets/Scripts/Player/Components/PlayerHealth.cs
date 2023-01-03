using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	[SerializeField] 
    private int startingHealth;
	private int currentHealth;
    private static bool isDead;
    public static PlayerHealth instance;
    
    void OnEnable () 	
    { 	
        instance = this; 	
    } 	
    
    void OnDisable () 	
    {
        instance = null; 	
    } 

	// Use this for initialization
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
        ManagerShake.instance.StartShakeRotating(0.09f, 5f);
        ManagerGUI.instance.HurtBlink();
    }

    private void Dead()
    {
        isDead = true;
        this.GetComponent<PlayerMovement>().enabled = false;
        this.GetComponentInChildren<SpriteRenderer>().enabled = false;
    }

    #region Properties

    public PlayerHealth Instance
    {
        get
        {
            return Instance;
        }

        set
        {
            Instance = value;
        }
    }
    
    #endregion

}
