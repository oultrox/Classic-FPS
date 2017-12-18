using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Attack")]
public class AttackAction : SM_Action
{
    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Attack();
    }
}
