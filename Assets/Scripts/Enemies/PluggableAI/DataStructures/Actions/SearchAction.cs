using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Search")]
public class SearchAction : SM_Action
{
    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Search();
    }

    public override void Initialize(EnemyStateMachine stateController)
    {
        stateController.Enemy.InitSearch();
    }
}
