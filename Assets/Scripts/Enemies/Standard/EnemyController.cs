using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using Enemies.Standard.InterfaceComponents;

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
        CacheBehavior<IEnemySearch>();
        CacheBehavior<IEnemyPatrol>();
        CacheBehavior<IEnemyWalk>();
        CacheBehavior<IEnemyChase>();
        CacheBehavior<IEnemyLook>();
        CacheBehavior<IEnemyAttack>();
        InjectTargets();
    }
    
    public void Tick<T>() where T : class, IEnemyTickable
    {
        GetBehavior<T>()?.Tick();
    }

    public bool IsLooking()
    {
        return GetBehavior<IEnemyLook>()?.IsLooking() ?? false;
    }
    
    private void CacheBehavior<T>() where T : class
    {
        var behavior = GetComponent<T>();
        if (behavior != null)
        {
            behaviors[typeof(T)] = behavior;
        }
    }

    private void InjectTargets()
    {
        List<IEnemyTargetable> targetables = GetComponents<IEnemyTargetable>().ToList();

        foreach (var t in targetables)
        {
            t.InjectTarget(playerTransform);
        }
    }
    
    private T GetBehavior<T>() where T : class
    {
        behaviors.TryGetValue(typeof(T), out var behavior);
        return behavior as T;
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
