using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySight : MonoBehaviour, IEnemyLook
{
    [Header("Enemy Sight")]
    [SerializeField] private float sightDistance = 10f;
    private Transform targetTransform;

    public void InjectTarget(Transform target)
    {
        targetTransform = target;
    }
    
    public bool IsLooking()
    {
        return (targetTransform.position - transform.position).sqrMagnitude < sightDistance * sightDistance;
    }
    
    public void AlertSight()
    {
        StartCoroutine(Alert());
    }

    private IEnumerator Alert()
    {
        sightDistance *= 4;
        yield return new WaitForSeconds(0.1f);
        sightDistance /= 4;
    }
}
