using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Action: ScriptableObject
{
    public enum UpdateType
    {
        Update,FixedUpdate,LateUpdate
    }

    public UpdateType updateType;
    public abstract void Act(EnemyStateMachine stateController);
}
