using UnityEngine;
using System;
using System.Collections.Generic;

/// <summary>
/// Modularized generic Enemy controller where we can slap behaviors at our will based on composition.
/// </summary>
public class EnemyController : MonoBehaviour 
{
    // TODO: Move health components out of here.
    [Header("Enemy Health")]
    [SerializeField] private int enemyHP = 100;
    [SerializeField] private int scoreValue = 10;
    [SerializeField] private GameObject corpse;

    [Header("Transitions (0 = infinite)")]
    [SerializeField] private float idleDuration = 1.5f;
    [SerializeField] private float walkDuration = 4;
    [SerializeField] private float searchDuration = 10f;

    private Transform playerTransform;
    private Dictionary<Type, object> behaviors = new();

    #region Properties
    public int ScoreValue { get => scoreValue; set => scoreValue = value; }
    public float IdleDuration { get => idleDuration; set => idleDuration = value; }
    public float WalkDuration { get => walkDuration; set => walkDuration = value; }
    public float SearchDuration { get => searchDuration; set => searchDuration = value; }
    #endregion

    private void Awake()
    {
        playerTransform = PlayerHealth.instance.GetComponent<Transform>();
        CacheBehavior<IEnemyAttack>();
        CacheBehavior<IEnemyPatrol>();
        CacheBehavior<IEnemyWalk>();
        CacheBehavior<IEnemyChase>();
        CacheBehavior<IEnemyLook>()?.SetTarget(playerTransform);
        CacheBehavior<IEnemySearch>();
    }

    private T CacheBehavior<T>() where T : class
    {
        var behavior = GetComponent<T>();
        if (behavior != null)
        {
            behaviors[typeof(T)] = behavior;
        }
        return behavior;
    }

    private T GetBehavior<T>() where T : class
    {
        behaviors.TryGetValue(typeof(T), out var behavior);
        return behavior as T;
    }

    public void Init<T>() where T : class, IEnemyBehaviour
    {
        GetBehavior<T>()?.Init();
    }
    public void Tick<T>() where T : class, IEnemyBehaviour
    {
        GetBehavior<T>()?.Tick();
    }

    public bool IsLooking()
    {
        return GetBehavior<IEnemyLook>()?.IsLooking() ?? false;
    }

    public void TakeDamage(int damage)
    {
        enemyHP -= damage;
        if (enemyHP <= 0) Die();
        else GetBehavior<IEnemyLook>()?.AlertSight();
    }

    private void Die()
    {
        if (corpse) Instantiate(corpse, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
