using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SM_Action: ScriptableObject
{
    public enum UpdateType
    {
        Update,FixedUpdate,LateUpdate
    }

    public UpdateType updateType;

    public abstract void Initialize(EnemyStateMachine stateController);
    public abstract void Act(EnemyStateMachine stateController);
}
