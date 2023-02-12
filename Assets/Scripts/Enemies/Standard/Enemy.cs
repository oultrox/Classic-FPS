using UnityEngine.AI;
using UnityEngine;
using System;


public class Enemy : MonoBehaviour 
{
    // TODO: Move to the health component.
    [Header("Life del enemigo")]
    [SerializeField] private int enemyHP = 100;
    [SerializeField] private int maxEnemyHP = 150;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private GameObject corpse;

    [Header("Transitions (0 it's infinite)")]
    [SerializeField] private float idleDuration = 1.5f;
    [SerializeField] private float walkDuration = 4;
    [SerializeField] private float searchDuration = 10f;
    
    // TODO: move to a look component I guess.
    [SerializeField] private float sightDistance = 10f;


    private NavMeshAgent navMesh;
    private Transform selfTransform;
    private Animator animator;

    private IEnemyLook look;
    private IEnemyAttack attack;
    private IEnemyPatrol patrol;
    private IEnemyWalk walk;
    private IEnemyChase chase;
    


    private  void Awake()
    {
        SelfTransform = GetComponent<Transform>();
        NavMesh = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        playerTransform = PlayerHealth.instance.GetComponent<Transform>();
        
        attack = GetComponent<IEnemyAttack>();
        patrol = GetComponent<IEnemyPatrol>();
        walk = GetComponent<IEnemyWalk>();
        chase = GetComponent<IEnemyChase>();
    }

    internal void InitChase()
    {
        chase.Init();
    }

    internal void InitWalk()
    {
        walk.Init();
    }

    internal void InitAttack()
    {
        attack.Init();
    }

    internal void InitPatrol()
    {
        patrol.Init();
    }

    public  bool IsLooking()
    {
        return (playerTransform.position - SelfTransform.position).sqrMagnitude < sightDistance * sightDistance;
    }

    public  void Walk()
    {
        walk.Tick();
    }

    public  void Chase()
    {
        chase.Tick();
    }

    public  void Patrol()
    {
        patrol.Tick();
    }

    public  void Attack()
    {
        attack.Tick();
    }

    public  void TakeDamage(int damage)
    {

    }
    public  void Die()
    {

    }


    #region Properties
    public int EnemyHP
    {
        get
        {
            return enemyHP;
        }

        set
        {
            enemyHP = value;
        }
    }

    public int MaxEnemyHP
    {
        get
        {
            return maxEnemyHP;
        }

        set
        {
            maxEnemyHP = value;
        }
    }

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

    public NavMeshAgent NavMesh
    {
        get
        {
            return navMesh;
        }

        set
        {
            navMesh = value;
        }
    }

    protected Transform SelfTransform { get => selfTransform; set => selfTransform = value; }
    protected Animator Animator { get => animator; set => animator = value; }

    private Transform playerTransform;
    #endregion
}
