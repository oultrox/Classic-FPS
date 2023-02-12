using UnityEngine.AI;
using UnityEngine;
using System;
using System.Collections;

public class EnemyController : MonoBehaviour 
{
    // TODO: Move to the health component.
    [Header("Enemy Health")]
    [SerializeField] private int enemyHP = 100;
    [SerializeField] private int maxEnemyHP = 150;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private GameObject corpse;

    // TODO: move to a look component I guess.
    [Header("Enemy Sight")]
    [SerializeField] private float sightDistance = 10f;

    [Header("Transitions (0 it's infinite)")]
    [SerializeField] private float idleDuration = 1.5f;
    [SerializeField] private float walkDuration = 4;
    [SerializeField] private float searchDuration = 10f;
    
    private Transform playerTransform;
    private IEnemyAttack attack;
    private IEnemyPatrol patrol;
    private IEnemyWalk walk;
    private IEnemyChase chase;
    

    private  void Awake()
    {
        playerTransform = PlayerHealth.instance.GetComponent<Transform>();
        
        attack = GetComponent<IEnemyAttack>();
        patrol = GetComponent<IEnemyPatrol>();
        walk = GetComponent<IEnemyWalk>();
        chase = GetComponent<IEnemyChase>();
    }

    internal void InitChase()
    {
        chase?.Init();
    }

    internal void InitWalk()
    {
        walk?.Init();
    }

    internal void InitAttack()
    {
        attack?.Init();
    }

    internal void InitPatrol()
    {
        patrol?.Init();
    }

    public  bool IsLooking()
    {
        return (playerTransform.position - transform.position).sqrMagnitude < sightDistance * sightDistance;
    }

    public  void Walk()
    {
        walk?.Tick();
    }

    public  void Chase()
    {
        chase?.Tick();
    }

    public  void Patrol()
    {
        patrol?.Tick();
    }

    public  void Attack()
    {
        attack?.Tick();
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
            StartCoroutine(Alert());
        }
    }

    private IEnumerator Alert()
    {
        sightDistance *= 4;
        yield return new WaitForSeconds(0.1f);
        sightDistance /= 4;
    }

    public void Die()
    {
        if (corpse)
        {
            Instantiate(corpse, transform.position, transform.rotation);
        }
        Destroy(gameObject);
    }


    #region Properties
    public int ScoreValue
    {
        get
        {
            return scoreValue;
        }

        set
        {
            scoreValue = value;
        }
    }

    public float IdleDuration
    {
        get
        {
            return idleDuration;
        }

        set
        {
            idleDuration = value;
        }
    }

    public float WalkDuration
    {
        get
        {
            return walkDuration;
        }

        set
        {
            walkDuration = value;
        }
    }

    public float SearchDuration
    {
        get
        {
            return searchDuration;
        }

        set
        {
            searchDuration = value;
        }
    }
    #endregion

}
