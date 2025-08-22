using System;
using System.Collections;
using System.Collections.Generic;
using Enemies.PluggableAI.DataStructures;
using Enemies.Standard.InterfaceComponents;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Chase")]
public class ChaseAction : SM_Action
{
    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Tick<IEnemyChase>();
    }
}
