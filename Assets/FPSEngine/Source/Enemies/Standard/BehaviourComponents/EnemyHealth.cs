using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, IHasHealth
{
    // TODO: Move health components out of here.
    [Header("Enemy Health")]
    [SerializeField] int enemyHP = 100;
    [SerializeField] int scoreValue = 10;
    [SerializeField] GameObject corpse;
    IEnemyLook _enemyLook;
    public int ScoreValue { get => scoreValue; set => scoreValue = value; }
    
    void Awake()
    {
        _enemyLook = GetComponent<IEnemyLook>();
    }
    
    public void TakeDamage(int damage)
    {
        enemyHP -= damage;
        if (enemyHP <= 0)
        {
            Die();
        }
        else
        {
            _enemyLook?.AlertSight();
        }
    }

    public Transform GetTransform()
    {
        return this.transform;;
    }

    private void Die()
    {
        if (corpse) Instantiate(corpse, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
