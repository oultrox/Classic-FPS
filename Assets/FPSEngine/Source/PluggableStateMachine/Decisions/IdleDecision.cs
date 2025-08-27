using Enemies.PluggableAI.DataStructures;
using Enemies.PluggableAI.DataStructures.Decisions;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Idle")]
public class IdleDecision : SM_Decision
{
    
    public override bool Decide(EnemyStateMachine stateMachine, IEnemyController stateController)
    {
        return stateMachine.HasStateElapsed(stateController.IdleDuration);
    }
}
