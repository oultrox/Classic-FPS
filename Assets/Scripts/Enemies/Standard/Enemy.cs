using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    [Header("Movimiento")]
    [SerializeField] protected float movementSpeed = 5;
    [SerializeField] protected float accelerationStep = 2f;

    [Header("Vida del enemigo")]
    [SerializeField] protected int enemyHP = 100;
    [SerializeField] protected int maxEnemyHP = 150;
    [SerializeField] protected int scoreValue = 10;

    [Header("Transiciones (0 lo deja continuo)")]
    [SerializeField] protected float idleDuration = 1.5f;
    [SerializeField] protected float walkDuration = 4;

    protected NavMeshAgent navMesh;
    protected Transform enemyTransform;

    public virtual void Awake()
    {
        enemyTransform = this.GetComponent<Transform>();
        navMesh = this.GetComponent<NavMeshAgent>();
    }
    public abstract bool IsLooking();
    public abstract void Walk();
    public abstract void Chase();
    public abstract void Attack();
    public abstract void TakeDamage(int damage);
    public abstract void Die();

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
    #endregion
}
