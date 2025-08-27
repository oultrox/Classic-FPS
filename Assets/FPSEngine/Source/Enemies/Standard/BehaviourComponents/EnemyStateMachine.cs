using Enemies.PluggableAI.DataStructures.States;

namespace Enemies.PluggableAI.DataStructures
{
    /// <summary>
    /// Usesd by the controller to set the states via the Plugglable IA with ScriptableObjects.
    /// </summary>
    public class EnemyStateMachine 
    {
        private SM_State _currentState;
        private SM_State _remainState;
        private float _stateTimeElapsed;
        
        public EnemyStateMachine(SM_State state, SM_State remainState)
        {
            _currentState = state;
            _remainState = remainState;
            _stateTimeElapsed = 0;
        }
    
        public void Tick(IEnemyController controller, float deltaTime)
        {
            _currentState.UpdateState(controller);
            _currentState.CheckTransitions(this, controller);
            _stateTimeElapsed += deltaTime;
        }

        public void FixedTick(IEnemyController controller)
        {
            _currentState.FixedUpdateState(controller);
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
            
            bool isCountDownElapsed = _stateTimeElapsed >= duration;
            return isCountDownElapsed;
        }
    }
}
