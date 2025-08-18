using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Patrol")]
public class PatrolAction : SM_Action
{
    public override void Initialize(EnemyStateMachine stateController)
    {
        stateController.Enemy.Init<IEnemyPatrol>();
    }

    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Tick<IEnemyPatrol>();
    }
}
