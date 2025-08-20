using Enemies.PluggableAI.DataStructures;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Attack")]
public class AttackAction : SM_Action
{
    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Tick<IEnemyAttack>();
    }
}
