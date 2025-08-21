using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using DumbInjector;
using Enemies.Standard.InterfaceComponents;

/// <summary>
/// Modularized generic Enemy controller where we can slap behaviors at our will based on composition.
/// </summary>
public class EnemyController : MonoBehaviour 
{
    [Header("Transitions (0 = infinite)")]
    [SerializeField] private float idleDuration = 1.5f;
    [SerializeField] private float walkDuration = 4;
    [SerializeField] private float searchDuration = 10f;

   
    [Inject] private IHasHealth playerHealth;
    private Transform playerTransform;
    private Dictionary<Type, object> behaviors = new();

    #region Properties
    public float IdleDuration { get => idleDuration; set => idleDuration = value; }
    public float WalkDuration { get => walkDuration; set => walkDuration = value; }
    public float SearchDuration { get => searchDuration; set => searchDuration = value; }
    #endregion
    
    private void Start()
    {
        playerTransform = playerHealth.GetTransform();
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
}
