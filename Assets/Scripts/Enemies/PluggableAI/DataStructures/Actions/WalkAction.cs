using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Walk")]
public class WalkAction : SM_Action
{
    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Walk();
    }
}
