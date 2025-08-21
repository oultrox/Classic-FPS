using UnityEngine;

namespace Enemies.Standard.InterfaceComponents
{
    public interface IEnemyTargetable
    {
        void InjectTarget(Transform target);
    }
}