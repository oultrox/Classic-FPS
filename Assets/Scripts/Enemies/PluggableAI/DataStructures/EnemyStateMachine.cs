using UnityEngine;

//Uses the enemy class to set the states via the Plugglable IA with ScriptableObjects.
public class EnemyStateMachine : MonoBehaviour {

    [SerializeField] private State currentState;
    [SerializeField] private State remainState;
    private Enemy enemy;
    private float stateTimeElapsed;

    //-------------------------------------------------
    // Métodos API
    //-------------------------------------------------
    private void Awake()
    {
        enemy = this.GetComponent<Enemy>();
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

    //-------------------------------------------------
    // Métodos Custom
    //-------------------------------------------------
    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            stateTimeElapsed = 0;
        }
    }

    public bool CheckIfCountDownElapsed(float duration)
    {
        if (duration <= 0)
        {
            return false;
        }

        stateTimeElapsed += Time.deltaTime;
        bool isCountDownElapsed = stateTimeElapsed >= duration;
        return isCountDownElapsed;
    }


    //-------------------------------------------------
    // Métodos Editor
    //-------------------------------------------------
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


    #region Properties
    public Enemy Enemy
    {
        get
        {
            return enemy;
        }

        set
        {
            enemy = value;
        }
    }

    public State CurrentState
    {
        get
        {
            return currentState;
        }

        set
        {
            currentState = value;
        }
    }

    public State RemainState
    {
        get
        {
            return remainState;
        }

        set
        {
            remainState = value;
        }
    }
    #endregion

}
