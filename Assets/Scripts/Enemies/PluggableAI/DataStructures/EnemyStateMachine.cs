using Enemies.PluggableAI.DataStructures.States;
using UnityEngine;

namespace Enemies.PluggableAI.DataStructures
{
    /// <summary>
    /// Uses the controller to set the states via the Plugglable IA with ScriptableObjects.
    /// </summary>
    [RequireComponent (typeof(EnemyController))]
    public class EnemyStateMachine : MonoBehaviour 
    {
        [SerializeField] private SM_State currentState;
        [SerializeField] private SM_State remainState;
        private float stateTimeElapsed;
        
        public EnemyController Enemy { get; set; }
        public SM_State CurrentState { get => currentState; set => currentState = value; }
        public SM_State RemainState { get => remainState; set => remainState = value; }
        
        
        private void Awake()
        {
            Enemy = GetComponent<EnemyController>();
            stateTimeElapsed = 0;
        }
    
        void Update()
        {
            currentState.UpdateState(this);
        }

        void FixedUpdate()
        {
            currentState.FixedUpdateState(this);
        }

        public void TransitionToState(SM_State nextState)
        {
            if (nextState != remainState)
            {
                currentState = nextState;
                stateTimeElapsed = 0;
            }
        }

        public bool HasStateElapsed(float duration)
        {
            if (duration <= 0)
            {
                return false;
            }

            stateTimeElapsed += Time.deltaTime;
            bool isCountDownElapsed = stateTimeElapsed >= duration;
            return isCountDownElapsed;
        }

        #if UNITY_EDITOR
        void OnDrawGizmos()
        {
            if (currentState != null)
            {
                Gizmos.color = currentState.sceneGizmoColor;
                Gizmos.DrawWireSphere(transform.position, 1f);
            }
        }
        #endif
    }
}
