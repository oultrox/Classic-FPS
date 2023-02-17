using UnityEngine;
using UnityEngine.AI;

[DisallowMultipleComponent]
public class EnemyWander : MonoBehaviour, IEnemyWalk
{
    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private float wanderSpeed = 5;
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
        navMesh.speed = wanderSpeed;
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

    // Can actually grab the IEnemyLook component and search more proficiently here if the enemy is not seeing the target.
    private void Wander()
    {
        waypointTimer = 0;
        // does nothing except pick a new destination to go to
        waypoint = new Vector3(UnityEngine.Random.Range(transform.position.x - walkRange, transform.position.x + walkRange),
                               0, UnityEngine.Random.Range(transform.position.z - walkRange, transform.position.z + walkRange));

        // if the waypoint is blocked the Walk method will call Wander() again to make sure the enemy follows a correct path.
        isWaypointBlocked = Physics.Linecast(transform.position, waypoint, raycastMask);

        float distance = (waypoint - transform.position).sqrMagnitude;
        waypointTravelTime = distance / (wanderSpeed * wanderSpeed);
    }

    #if UNITY_EDITOR
    void OnDrawGizmos()
    {
        if (waypoint != Vector3.zero && !isWaypointBlocked)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(waypoint, 1f);
        }
    }
    #endif

}
