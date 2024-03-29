﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Chase")]
public class ChaseAction : SM_Action
{
    public GameObject gameObjectTest;
    public override void Initialize(EnemyStateMachine stateController)
    {
        stateController.Enemy.InitChase();
    }

    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Chase();
    }
}
