using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Search(Wait)")]
public class WaitSearchDecision : Decision
{
    public override bool Decide(EnemyStateMachine stateController)
    {
        return stateController.CheckIfCountDownElapsed(stateController.Enemy.SearchTime);
    }
}
