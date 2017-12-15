using System;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : Decision
{
    public override bool Decide(EnemyStateMachine stateController)
    {
        return stateController.Enemy.IsLooking();
    }
}
