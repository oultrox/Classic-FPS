using Enemies.PluggableAI.DataStructures;
using Enemies.Standard.InterfaceComponents;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Attack")]
public class AttackAction : SM_Action
{
    public override void Act(IEnemyController stateController)
    {
        stateController.TickEnemyComponent<IEnemyAttack>();
    }
}
