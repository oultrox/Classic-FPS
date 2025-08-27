using FPS.Scripts.Enemies.Standard;
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

        public void StartState(IEnemyController stateController)
        {
        }
    
        public void UpdateState(IEnemyController controller)
        {
            DoAction(controller, SM_Action.UpdateType.Update);
        }

        public void FixedUpdateState(IEnemyController controller)
        {
            DoAction(controller, SM_Action.UpdateType.FixedUpdate);
        }

        public void LateUpdateState(IEnemyController stateController)
        {
            DoAction(stateController, SM_Action.UpdateType.LateUpdate);
        }

        private void DoAction(IEnemyController stateController, SM_Action.UpdateType updateType)
        {
            for (int i = 0; i < actions.Length; i++)
            {
                if (updateType == actions[i].updateType)
                {
                    actions[i].Act(stateController);
                }
            }
        }

        public void CheckTransitions(EnemyStateMachine stateController, IEnemyController enemyController)
        {
            for (int i = 0; i < transitions.Length; i++)
            {
                decisionSucceeded = transitions[i].decision.Decide(stateController, enemyController);

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
