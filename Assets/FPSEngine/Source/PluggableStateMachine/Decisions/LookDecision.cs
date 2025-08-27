using System;
using Enemies.PluggableAI.DataStructures;
using Enemies.PluggableAI.DataStructures.Decisions;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Decisions/Look")]
public class LookDecision : SM_Decision
{
    public override bool Decide(EnemyStateMachine stateMachine, IEnemyController stateController)
    {
        return stateController.HasSight();
    }
}
