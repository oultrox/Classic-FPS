using Enemies.Standard.InterfaceComponents;
using UnityEngine;
public interface IEnemyLook: IEnemyTargetable
{
    public bool IsLooking();
    public void AlertSight();
}