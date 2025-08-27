using UnityEngine;

namespace Enemies.PluggableAI.DataStructures.Decisions
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Walk")]
    public class WalkDecision : SM_Decision
    {
        public override bool Decide(EnemyStateMachine stateMachine, IEnemyController stateController)
        {
            return stateMachine.HasStateElapsed(stateController.WalkDuration);
        }
    }
}