using UnityEngine;
using System;

public class EnemyController : MonoBehaviour 
{
    // TODO: Move to the health component.
    [Header("Enemy Health")]
    [SerializeField] private int enemyHP = 100;
    [SerializeField] private int maxEnemyHP = 150;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private GameObject corpse;


    [Header("Transitions (0 it's infinite)")]
    [SerializeField] private float idleDuration = 1.5f;
    [SerializeField] private float walkDuration = 4;
    [SerializeField] private float searchDuration = 10f;
    
    private Transform playerTransform;
    private IEnemyLook look;
    private IEnemyAttack attack;
    private IEnemyPatrol patrol;
    private IEnemyWalk walk;
    private IEnemyChase chase;
    private IEnemySearch search;

    private  void Awake()
    {
        playerTransform = PlayerHealth.instance.GetComponent<Transform>();
        
        attack = GetComponent<IEnemyAttack>();
        patrol = GetComponent<IEnemyPatrol>();
        walk = GetComponent<IEnemyWalk>();
        chase = GetComponent<IEnemyChase>();
        look = GetComponent<IEnemyLook>();
        look?.SetTarget(playerTransform);
        search = GetComponent<IEnemySearch>();
    }

    public void InitChase()
    {
        chase?.Init();
    }

    public void InitWalk()
    {
        walk?.Init();
    }

    public void InitAttack()
    {
        attack?.Init();
    }

    public void InitPatrol()
    {
        patrol?.Init();
    }
    public void InitSearch()
    {
        search?.Init();
    }


    public bool IsLooking()
    {
        if (look == null)
            return false;

        return look.IsLooking();
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

    public void Search()
    {
        search?.Tick();
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
           look?.AlertSight();
        }
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
