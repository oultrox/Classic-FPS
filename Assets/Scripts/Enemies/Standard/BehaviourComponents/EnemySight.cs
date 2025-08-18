using System.Collections;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemySight : MonoBehaviour, IEnemyLook
{
    [Header("Enemy Sight")]
    [SerializeField] private float sightDistance = 10f;
    private Transform targetTransform;

    public bool IsLooking()
    {
        return (targetTransform.position - transform.position).sqrMagnitude < sightDistance * sightDistance;
    }

    public void SetTarget(Transform target)
    {
        targetTransform = target;
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

    public void Init()
    {
    }

    public void Tick()
    {
    }
}
