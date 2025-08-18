using UnityEngine;
public interface IEnemyLook : IEnemyBehaviour
{
    public void SetTarget(Transform transform);
    public bool IsLooking();
    public void AlertSight();
}