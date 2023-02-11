using UnityEngine.AI;
using UnityEngine;
using System;


// TO DO: even better, avoid inheritance and uses components for looking, walking, patrolling, attacking with Interfaces for polymorphysm.
public abstract class Enemy : MonoBehaviour {

    [Header("====== Enemy ======")]
    [Header("Movimiento")]
    [SerializeField] protected float movementSpeed = 5;
    [SerializeField] protected float accelerationStep = 2f;
    [SerializeField] protected float minDistanceChase = 3f;

    [Header("Vida del enemigo")]
    [SerializeField] protected int enemyHP = 100;
    [SerializeField] protected int maxEnemyHP = 150;
    [SerializeField] protected int scoreValue = 10;
    [SerializeField] protected GameObject corpse;

    [Header("Transiciones (0 lo deja continuo)")]
    [SerializeField] protected float idleDuration = 1.5f;
    [SerializeField] protected float walkDuration = 4;
    [SerializeField] private float searchDuration = 10f;

    protected NavMeshAgent navMesh;
    private Transform selfTransform;
    private Animator animator;

    public virtual void Awake()
    {
        SelfTransform = GetComponent<Transform>();
        NavMesh = GetComponent<NavMeshAgent>();
        Animator = GetComponent<Animator>();
        NavMesh.speed = MovementSpeed;
    }
    public abstract bool IsLooking();
    public abstract void Walk();
    public abstract void Chase();
    public abstract void Patrol();
    public abstract void Attack();
    public abstract void TakeDamage(int damage);
    public abstract void Die();

    internal void InitChase()
    {
        throw new NotImplementedException();
    }

    internal void InitWalk()
    {
        throw new NotImplementedException();
    }

    internal void InitAttack()
    {
        throw new NotImplementedException();
    }

    internal void InitPatrol()
    {
        throw new NotImplementedException();
    }

    #region Properties
    public float MovementSpeed
    {
        get
        {
            return movementSpeed;
        }

        set
        {
            movementSpeed = value;
        }
    }

    public float AccelerationStep
    {
        get
        {
            return accelerationStep;
        }

        set
        {
            accelerationStep = value;
        }
    }

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
    #endregion
}
