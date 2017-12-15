using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Walk")]
public class WalkDecision : Decision
{
    public override bool Decide(EnemyStateMachine stateController)
    {
        return stateController.CheckIfCountDownElapsed(stateController.Enemy.WalkDuration);
    }
}