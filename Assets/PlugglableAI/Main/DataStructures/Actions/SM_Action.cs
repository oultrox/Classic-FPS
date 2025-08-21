using System.Collections;
using System.Collections.Generic;
using Enemies.PluggableAI.DataStructures;
using UnityEngine;

public abstract class SM_Action: ScriptableObject
{
    public enum UpdateType
    {
        Update,FixedUpdate,LateUpdate
    }
    public UpdateType updateType;
    public abstract void Act(EnemyStateMachine stateController);
}
