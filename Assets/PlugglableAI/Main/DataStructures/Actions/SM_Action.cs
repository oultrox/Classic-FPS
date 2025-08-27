using System.Collections;
using System.Collections.Generic;
using Enemies.PluggableAI.DataStructures;
using Enemies.Standard.InterfaceComponents;
using FPS.Scripts.Enemies.Standard;
using UnityEngine;

public abstract class SM_Action: ScriptableObject
{
    public enum UpdateType
    {
        Update,FixedUpdate,LateUpdate
    }
    public UpdateType updateType;
    public abstract void Act(IEnemyController stateController);
}

public interface IEnemyController
{
    public void TickEnemyComponent<T>() where T : class, IEnemyTickable;
    public bool HasSight();
    float IdleDuration { get; set; }
    float WalkDuration { get; set; }
    float SearchDuration { get; set; }
}
