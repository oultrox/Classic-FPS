using UnityEngine;

namespace Enemies.PluggableAI.DataStructures.Decisions
{
    public abstract class SM_Decision : ScriptableObject
    {
        public abstract bool Decide(EnemyStateMachine stateMachine, IEnemyController stateController);

    }
}
