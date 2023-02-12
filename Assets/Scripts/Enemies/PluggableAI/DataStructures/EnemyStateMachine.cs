using System;
using UnityEngine;

//Uses the enemy class to set the states via the Plugglable IA with ScriptableObjects.
public class EnemyStateMachine : MonoBehaviour {

    [SerializeField] private State currentState;
    [SerializeField] private State remainState;
    private EnemyController enemy;
    private float stateTimeElapsed;

    private void Awake()
    {
        enemy = GetComponent<EnemyController>();
        stateTimeElapsed = 0;
    }

    private void Start()
    {
        currentState.StartState(this);
    }

    void Update()
    {
        currentState.UpdateState(this);
    }

    void FixedUpdate()
    {
        currentState.FixedUpdateState(this);
    }

    public void TransitionToState(State nextState)
    {
        if (nextState != remainState)
        {
            currentState = nextState;
            currentState.StartState(this);
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
    public EnemyController Enemy
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
