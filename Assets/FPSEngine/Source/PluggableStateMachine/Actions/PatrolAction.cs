using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.PluggableAI.DataStructures;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Patrol")]
public class PatrolAction : SM_Action
{
    public override void Act(IEnemyController stateController)
    {
        stateController.TickEnemyComponent<IEnemyPatrol>();
    }
}
