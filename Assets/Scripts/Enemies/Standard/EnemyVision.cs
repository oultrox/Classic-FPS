using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyVision : MonoBehaviour {

    Vector3 destination;
    private NavMeshAgent enemyNavMesh;
    private bool movedOnce = false;
    private void Awake()
    {
        enemyNavMesh = this.GetComponentInParent<NavMeshAgent>();
    }

    public void Update()
    {
        if (enemyNavMesh.velocity == Vector3.zero && movedOnce == false)
        {
            destination = Vector3.zero;
        }
        else
        {
            movedOnce = true;
            destination = enemyNavMesh.destination;
        }
        transform.LookAt(destination);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward * 10000f);
    }
}
