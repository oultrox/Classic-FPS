using UnityEngine;

namespace Enemies.PluggableAI.DataStructures.States
{
    [CreateAssetMenu(menuName = "PluggableAI/State")]
    public class SM_State : ScriptableObject
    {
        public SM_Action[] actions;
        public Transition[] transitions;
        public Color sceneGizmoColor = Color.grey;

        private bool decisionSucceeded;

        public void StartState(EnemyStateMachine stateController)
        {
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
                decisionSucceeded = transitions[i].decision.Decide(stateController);

                if (decisionSucceeded && transitions[i].trueState != null)
                {
                    stateController.TransitionToState(transitions[i].trueState);
                }
                else if (!decisionSucceeded && transitions[i].falseState != null)
                {
                    stateController.TransitionToState(transitions[i].falseState);
                }
            }
        }
    }
}
