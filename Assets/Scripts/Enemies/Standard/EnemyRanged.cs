using System.Collections;
using UnityEngine;

public class EnemyRanged : Enemy
{
    [Header("====== Ranged ======")]
    [Header("Proyectil")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletDamage = 20;
    [SerializeField] private float bulletSpeed = 10;

    [Header("Attack")]
    [SerializeField] private float attackRate = 1;
    [SerializeField] private float sightDistance = 10f;

    [Header("Vision")]
    [SerializeField] private float viewAngle;
    [SerializeField] private LayerMask raycastMask;

    [Header("Patrol (5 is minimum)")]
    [Min(5)]
    [SerializeField] private float walkRange = 5;
    [SerializeField] private Transform[] patrolWayPoints;
    

    private GameObject bulletObj;
    private Transform playerTransform;
    private float attackTimer;
    private int nextWaypoint = 0;
    
    private Vector3 waypoint;
    private bool isWaypointBlocked;
    private float waypointTimer;
    private float waypointTravelTime;



    public override void Awake()
    {
        base.Awake();
        playerTransform = PlayerHealth.instance.GetComponent<Transform>(); 
        Sexar();
    }

    public new void Sexar()
    {
        Debug.Log("H0li B");
    }

    public void Update()
    {
        if (NavMesh.remainingDistance <= 1.0f)
        {
           Animator.SetBool("isMoving", false);
           Animator.SetBool("isMovingSides", false);
        }
    }

    //------------------------------------------------------------------
    // Enemy methods
    //------------------------------------------------------------------

    public override bool IsLooking()
    {
       return (playerTransform.position - SelfTransform.position).sqrMagnitude < sightDistance * sightDistance;
    }

    public override void Patrol()
    {
        NavMesh.destination = patrolWayPoints[nextWaypoint].position;
        NavMesh.isStopped = false;
        Animator.SetBool("isMoving", !NavMesh.isStopped);
        if (NavMesh.remainingDistance <= NavMesh.stoppingDistance && !NavMesh.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % patrolWayPoints.Length;
        }
    }

    public override void Walk()
    {

        waypointTimer += Time.deltaTime;

        float distanceBetweenWayPoint = (waypoint - SelfTransform.position).sqrMagnitude;
        if (distanceBetweenWayPoint < (1 * 1) || isWaypointBlocked || waypointTimer > waypointTravelTime)
        {
            Animator.SetBool("isMoving", false);
            Animator.SetBool("isMovingSides", false);
            Wander();
        }
        else
        {
            Animator.SetBool("isMoving", true);
            NavMesh.destination = waypoint;
            NavMesh.isStopped = false;
        }
        
    }

    public override void Chase()
    {
        if (!NavMesh.pathPending && (playerTransform.position - SelfTransform.position).sqrMagnitude > minDistanceChase * minDistanceChase)
        {
            NavMesh.destination = playerTransform.position;
            NavMesh.isStopped = false;
            Animator.SetBool("isMoving", true);
            
        }
        if (NavMesh.remainingDistance < minDistanceChase)
        {
            NavMesh.isStopped = true;
            Animator.SetBool("isMoving", false);
            Animator.SetBool("isMovingSides", false);
        }
    } 

    public override void Attack()
    {
        attackTimer += Time.deltaTime;

        bool canEnemyAttack = attackTimer >= attackRate && enemyHP > 0;
        if (!canEnemyAttack)
        {
            return;
        }

        //Reinicio del timer.
        attackTimer = 0f;
        Shoot(0);
    }

    public override void TakeDamage(int damage)
    {
        enemyHP -= damage;
        if (enemyHP <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Alert());
        }
        
    }

    public override void Die()
    {
        //Checking for testing purposes.
        if (corpse)
        {
            Instantiate(corpse, SelfTransform.position, SelfTransform.rotation);
        }
        Destroy(gameObject);
    }

    //------------------------------------------------------------------
    // Private methods that helps the Enemy implementation
    //------------------------------------------------------------------

    private IEnumerator Alert()
    {
        sightDistance *= 4;
        yield return new WaitForSeconds(0.1f);
        sightDistance /= 4;
    }

    private void Shoot(float anguloDisparo)
    {
        Vector3 direction = playerTransform.position - SelfTransform.position;
        direction = Quaternion.Euler(anguloDisparo, anguloDisparo, 0 ) * direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction = direction.normalized;

        bulletObj = Instantiate(bulletPrefab, SelfTransform.position, Quaternion.AngleAxis(angle, Vector3.forward)) as GameObject;
        bulletObj.GetComponent<BulletEnemy>().Init(bulletDamage, direction, bulletSpeed);
    }

    // Create a new way point target
    private void Wander()
    {
        waypointTimer = 0;
        // does nothing except pick a new destination to go to
        waypoint = new Vector3(UnityEngine.Random.Range(SelfTransform.position.x - walkRange, SelfTransform.position.x + walkRange),
                               0, UnityEngine.Random.Range(SelfTransform.position.z - walkRange, SelfTransform.position.z + walkRange));

        // if the waypoint is blocked the Walk method will call Wander() again to make sure the enemy follows a correct path.
        isWaypointBlocked = Physics.Linecast(SelfTransform.position, waypoint,raycastMask);

        float distance = (waypoint - SelfTransform.position).sqrMagnitude;
        waypointTravelTime = distance / (movementSpeed * movementSpeed);
    }

    //-------------------------------------------------
    // Métodos Editor
    //-------------------------------------------------

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
