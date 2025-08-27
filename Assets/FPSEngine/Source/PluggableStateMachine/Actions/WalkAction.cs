using Enemies.PluggableAI.DataStructures;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Walk")]
public class WalkAction : SM_Action
{
    public override void Act(IEnemyController stateController)
    {
        stateController.TickEnemyComponent<IEnemyWalk>();
    }
}
