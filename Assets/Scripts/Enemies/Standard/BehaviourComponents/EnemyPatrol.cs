using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemyPatrol : MonoBehaviour, IEnemyPatrol
{
    [SerializeField] private float patrolSpeed = 5;
    [Header("Patrol Waypoints")]
    [SerializeField] private Transform[] patrolWayPoints;
    
    private NavMeshAgent navMesh;
    private int nextWaypoint = 0;

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }
    public void Init()
    {
        navMesh.speed = patrolSpeed;
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
