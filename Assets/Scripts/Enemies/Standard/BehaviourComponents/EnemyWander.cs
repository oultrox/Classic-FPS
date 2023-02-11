﻿using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyWander : MonoBehaviour,IEnemyWalk
{
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private float movementSpeed = 5;
    [SerializeField] private float walkRange = 5;
    private float waypointTimer;
    private NavMeshAgent navMesh;
    private Vector3 waypoint;
    private bool isWaypointBlocked;
    private float waypointTravelTime;
   

    private void Awake()
    {
        navMesh = GetComponent<NavMeshAgent>();
    }

    public void Init()
    {

    }

    public void Tick()
    {
        waypointTimer += Time.deltaTime;

        float distanceBetweenWayPoint = (waypoint - transform.position).sqrMagnitude;
        if (distanceBetweenWayPoint < (1 * 1) || isWaypointBlocked || waypointTimer > waypointTravelTime)
        {
            Wander();
        }
        else
        {
            navMesh.destination = waypoint;
            navMesh.isStopped = false;
        }
    }

    private void Wander()
    {
        waypointTimer = 0;
        // does nothing except pick a new destination to go to
        waypoint = new Vector3(UnityEngine.Random.Range(transform.position.x - walkRange, transform.position.x + walkRange),
                               0, UnityEngine.Random.Range(transform.position.z - walkRange, transform.position.z + walkRange));

        // if the waypoint is blocked the Walk method will call Wander() again to make sure the enemy follows a correct path.
        isWaypointBlocked = Physics.Linecast(transform.position, waypoint, raycastMask);

        float distance = (waypoint - transform.position).sqrMagnitude;
        waypointTravelTime = distance / (movementSpeed * movementSpeed);
    }
}
