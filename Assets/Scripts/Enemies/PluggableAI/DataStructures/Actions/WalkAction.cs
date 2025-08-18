using UnityEngine;

[CreateAssetMenu(menuName = "PluggableAI/SM_Actions/Walk")]
public class WalkAction : SM_Action
{
    public override void Initialize(EnemyStateMachine stateController)
    {
        stateController.Enemy.Init<IEnemyWalk>();
    }

    public override void Act(EnemyStateMachine stateController)
    {
        stateController.Enemy.Tick<IEnemyWalk>();
    }

  
}
