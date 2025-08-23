using DumbInjector;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IHasHealth
{
	[SerializeField] private int startingHealth;
    [Inject] CameraShaker playerCameraShaker;
    
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
        playerCameraShaker.StartShakeRotating(0.09f, 5f);
        ManagerGUI.instance.HurtBlink();
    }

    public Transform GetTransform()
    {
        return transform;
    }

    private void Dead()
    {
        isDead = true;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponentInChildren<SpriteRenderer>().enabled = false;
    }
}
