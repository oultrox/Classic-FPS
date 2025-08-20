using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemyChase : MonoBehaviour,IEnemyChase
{
    [SerializeField] private float chaseSpeed = 5;
    [SerializeField] private float minDistanceChase = 3f;
    private NavMeshAgent navMesh;
    private Transform playerTransform;

    public void InjectTarget(Transform target)
    {
        navMesh = GetComponent<NavMeshAgent>();
        playerTransform = PlayerHealth.instance.GetComponent<Transform>();
        navMesh.speed = chaseSpeed;
    }
    
    public void Tick()
    {
        if (!navMesh.pathPending && (playerTransform.position - transform.position).sqrMagnitude > minDistanceChase * minDistanceChase)
        {
            navMesh.destination = playerTransform.position;
            navMesh.isStopped = false;

        }
        if (navMesh.remainingDistance < minDistanceChase)
        {
            navMesh.isStopped = true;
        }
    }

        
}
