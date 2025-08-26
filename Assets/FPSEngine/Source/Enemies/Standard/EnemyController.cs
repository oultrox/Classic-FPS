using System;
using System.Collections.Generic;
using System.Linq;
using DumbInjector;
using Enemies.PluggableAI.DataStructures;
using Enemies.PluggableAI.DataStructures.States;
using Enemies.Standard.InterfaceComponents;
using UnityEngine;

namespace FPS.Scripts.Enemies.Standard
{
    /// <summary>
    /// Modularized generic Enemy controller where we can slap behaviors at our will based on composition.
    /// </summary>
    public class EnemyController : MonoBehaviour 
    {
        [Header("Transitions (0 = infinite)")]
        [SerializeField] private float idleDuration = 1.5f;
        [SerializeField] private float walkDuration = 4;
        [SerializeField] private float searchDuration = 10f;
        [SerializeField] private SM_State currentState;
        [SerializeField] private SM_State remainState;
        
        [Inject] private IHasHealth playerHealth;
    
        private Transform playerTransform;
        private EnemyStateMachine enemyStateMachine;
        private Dictionary<Type, object> behaviors = new();

        #region Properties
        public float IdleDuration { get => idleDuration; set => idleDuration = value; }
        public float WalkDuration { get => walkDuration; set => walkDuration = value; }
        public float SearchDuration { get => searchDuration; set => searchDuration = value; }
        public SM_State CurrentState => currentState;
        public SM_State RemainState => remainState;
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
            enemyStateMachine = new EnemyStateMachine(this);
        }

        private void Update()
        {
            enemyStateMachine.Tick();
        }

        private void FixedUpdate()
        {
            enemyStateMachine.FixedTick();
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
        
#if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (CurrentState != null)
            {
                Gizmos.color = CurrentState.sceneGizmoColor;
                Gizmos.DrawWireSphere(transform.position, 1f);
            }
        }
#endif
    }
}
