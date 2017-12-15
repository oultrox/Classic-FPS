using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/State")]
public class State : ScriptableObject
{

    public Action[] actions;
    public Transition[] transitions;
    public Color sceneGizmoColor = Color.grey;

    private bool decisionSucceded;

    public void UpdateState(EnemyStateMachine stateController)
    {
        DoAction(stateController, Action.UpdateType.Update);
    }

    public void FixedUpdateState(EnemyStateMachine stateController)
    {
        DoAction(stateController, Action.UpdateType.FixedUpdate);
    }

    public void LateUpdateState(EnemyStateMachine stateController)
    {
        DoAction(stateController, Action.UpdateType.LateUpdate);
    }

    private void DoAction(EnemyStateMachine stateController, Action.UpdateType updateType)
    {
        for (int i = 0; i < actions.Length; i++)
        {
            if (updateType == actions[i].updateType)
            {
                actions[i].Act(stateController);
            }
        }
        CheckTransitions(stateController);
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
