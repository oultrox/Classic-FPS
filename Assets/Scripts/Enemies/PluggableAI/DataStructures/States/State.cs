using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public class State : ScriptableObject
{

    public SM_Action[] actions;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    private bool decisionSucceded;

    public void StartState(EnemyStateMachine stateController)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            actions[i].Initialize(stateController);
        }
    }
    public void UpdateState(EnemyStateMachine stateController)
    {
        DoAction(stateController, SM_Action.UpdateType.Update);
        CheckTransitions(stateController);
    }

    public void FixedUpdateState(EnemyStateMachine stateController)
    {
        DoAction(stateController, SM_Action.UpdateType.FixedUpdate);
    }

    public void LateUpdateState(EnemyStateMachine stateController)
    {
        DoAction(stateController, SM_Action.UpdateType.LateUpdate);
    }

    private void DoAction(EnemyStateMachine stateController, SM_Action.UpdateType updateType)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            if (updateType == actions[i].updateType)
            {
                actions[i].Act(stateController);
            }
        }
    }

    private void CheckTransitions(EnemyStateMachine stateController)
    {
        for (int i = 0; i < transitions.Length; i++)
        {
            decisionSucceded = transitions[i].decision.Decide(stateController);

            if (decisionSucceded)
            {
                stateController.TransitionToState(transitions[i].trueState);
            }
            else
            {
                stateController.TransitionToState(transitions[i].falseState);
            }
        }
    }
}
