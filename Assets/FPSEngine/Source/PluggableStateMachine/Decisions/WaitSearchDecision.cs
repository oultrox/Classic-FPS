using UnityEngine;

namespace Enemies.PluggableAI.DataStructures.Decisions
{
    [CreateAssetMenu(menuName = "PluggableAI/Decisions/Search(Wait)")]
    public class WaitSearchDecision : SM_Decision
    {
        public override bool Decide(EnemyStateMachine stateMachine, IEnemyController stateController)
        {
            return stateMachine.HasStateElapsed(stateController.SearchDuration);
        }
    }
}
