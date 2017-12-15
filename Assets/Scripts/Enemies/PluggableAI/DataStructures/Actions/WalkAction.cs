using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/Actions/Walk")]
public class WalkAction : Action
{
    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Walk();
    }
}
