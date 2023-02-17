using UnityEngine;

// Component Contracts
public interface IEnemyLook 
{
    public void SetTarget(Transform transform);
    public abstract bool IsLooking();
    public void AlertSight();
}