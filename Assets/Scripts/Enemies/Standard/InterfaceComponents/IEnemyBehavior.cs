using UnityEngine;

public interface IEnemyTickable
{
    void Tick();
}

public interface IEnemyTargetable
{
    void InjectTarget(Transform target);
}