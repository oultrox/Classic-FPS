using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Search")]
public class SearchAction : SM_Action
{
    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Init<IEnemySearch>();
    }

    public override void Initialize(EnemyStateMachine stateController)
    {
        stateController.Enemy.Tick<IEnemySearch>();
    }
}
