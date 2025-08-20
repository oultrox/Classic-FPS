using UnityEngine;

namespace Enemies.PluggableAI.DataStructures.Decisions
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Walk")]
    public class WalkDecision : SM_Decision
    {
        public override bool Decide(EnemyStateMachine stateController)
        {
            return stateController.HasStateElapsed(stateController.Enemy.WalkDuration);
        }
    }
}