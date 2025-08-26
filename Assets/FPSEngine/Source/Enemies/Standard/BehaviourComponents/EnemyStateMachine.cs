using Enemies.PluggableAI.DataStructures.States;
using FPS.Scripts.Enemies.Standard;
using UnityEngine;

namespace Enemies.PluggableAI.DataStructures
{
    /// <summary>
    /// Uses the controller to set the states via the Plugglable IA with ScriptableObjects.
    /// </summary>
    public class EnemyStateMachine 
    {
        private SM_State _currentState;
        private SM_State _remainState;
        private float _stateTimeElapsed;
        
        public EnemyController Enemy { get; }
        
        public EnemyStateMachine(EnemyController enemy)
        {
            Enemy = enemy;
            _currentState = Enemy.CurrentState;
            _remainState = Enemy.RemainState;
            _stateTimeElapsed = 0;
        }
    
        public void Tick()
        {
            _currentState.UpdateState(this);
        }

        public void FixedTick()
        {
            _currentState.FixedUpdateState(this);
        }

        public void TransitionToState(SM_State nextState)
        {
            if (nextState != _remainState)
            {
                _currentState = nextState;
                _stateTimeElapsed = 0;
            }
        }

        public bool HasStateElapsed(float duration)
        {
            if (duration <= 0)
            {
                return false;
            }

            _stateTimeElapsed += Time.deltaTime;
            bool isCountDownElapsed = _stateTimeElapsed >= duration;
            return isCountDownElapsed;
        }
    }
}
