using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour, IEnemyPatrol
{
    [Header("Patrol (5 is minimum)")]
    [Min(5)]
    [SerializeField] private float walkRange = 5;
    [SerializeField] private Transform[] patrolWayPoints;
    private NavMeshAgent navMesh;
    private int nextWaypoint = 0;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    public void Init()
    {

    }

    public void Tick()
    {
        navMesh.destination = patrolWayPoints[nextWaypoint].position;
        navMesh.isStopped = false;
        
        if (navMesh.remainingDistance <= navMesh.stoppingDistance && !navMesh.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % patrolWayPoints.Length;
        }
    }

}
