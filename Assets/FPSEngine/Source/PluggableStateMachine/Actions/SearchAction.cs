using Enemies.PluggableAI.DataStructures;
using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Search")]
public class SearchAction : SM_Action
{
    public override void Act(IEnemyController stateController)
    {
        stateController.TickEnemyComponent<IEnemySearch>();
    }
}
