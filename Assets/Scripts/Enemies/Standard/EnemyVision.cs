using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyVision : MonoBehaviour {

    Vector3 destination;
    private NavMeshAgent enemyNavMesh;

    private void Awake()
    {
        enemyNavMesh = GetComponentInParent<NavMeshAgent>();
    }
    public void Update()
    {
        destination = enemyNavMesh.destination;
        transform.LookAt(destination);
    }
}
