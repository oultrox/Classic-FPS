using DumbInjector;
using Enemies.PluggableAI.DataStructures;
using Enemies.PluggableAI.DataStructures.States;
using Enemies.Standard.InterfaceComponents;
using FPS.Scripts.Enemies.Standard;
using UnityEngine;

namespace FPSEngine.Source.Enemies.Standard
{
    public class EnemyController : MonoBehaviour, IEnemyController
    {
        [Header("Transitions (0 = infinite)")]
        [SerializeField] private float idleDuration = 1.5f;
        [SerializeField] private float walkDuration = 4f;
        [SerializeField] private float searchDuration = 10f;
        [SerializeField] private SM_State currentState;
        [SerializeField] private SM_State remainState;
        [Inject] private IHasHealth playerHealth;

        private Transform targetTransform;
        private EnemyStateMachine stateMachine;
        private BehaviorRegistry registry;

        #region Properties
        public float IdleDuration { get => idleDuration; set => idleDuration = value; }
        public float WalkDuration { get => walkDuration; set => walkDuration = value; }
        public float SearchDuration { get => searchDuration; set => searchDuration = value; }
        #endregion

        private void Start()
        {
            SetBehavioralComponents();
        }
        
        private void Update()
        {
            stateMachine.Tick(this, Time.deltaTime);
        }

        private void FixedUpdate()
        {
            stateMachine.FixedTick(this);
        }

        public void TickEnemyComponent<T>() where T : class, IEnemyTickable
        {
            registry.Get<T>()?.Tick();
        }

        public bool HasSight()
        {
            return registry.Get<IEnemyLook>()?.IsLooking() ?? false;
        } 

        private void SetBehavioralComponents()
        {
            var possibleBehaviorTypes = new[]
            {
                typeof(IEnemySearch),
                typeof(IEnemyPatrol),
                typeof(IEnemyWalk),
                typeof(IEnemyChase),
                typeof(IEnemyLook),
                typeof(IEnemyAttack)
            };
            
            targetTransform = playerHealth.GetTransform();
            registry = new BehaviorRegistry(gameObject, possibleBehaviorTypes);
            stateMachine = new EnemyStateMachine(currentState, remainState);
            
            foreach (var t in registry.GetAllOfType<IEnemyTargetable>())
            {
                t.InjectTarget(targetTransform);
            }
        }

#if UNITY_EDITOR
        private void OnDrawGizmos()
        {
            if (currentState == null) return;
            Gizmos.color = currentState.sceneGizmoColor;
            Gizmos.DrawWireSphere(transform.position, 1f);
        }
#endif
    }
}
