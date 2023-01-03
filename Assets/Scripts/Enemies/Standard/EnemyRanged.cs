using System;
using System.Collections;
using System.Collections.Generic;
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
    private Animator anim;



    public override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        playerTransform = PlayerHealth.instance.GetComponent<Transform>();
    }

    public void Update()
    {
        if (this.NavMesh.remainingDistance <= 1.0f)
        {
           anim.SetBool("isMoving", false);
           anim.SetBool("isMovingSides", false);
        }
    }

    //------------------------------------------------------------------
    // Enemy methods
    //------------------------------------------------------------------

    public override bool IsLooking()
    {
       return (playerTransform.position - selfTransform.position).sqrMagnitude < sightDistance * sightDistance;
    }

    public override void Patrol()
    {
        this.NavMesh.destination = patrolWayPoints[nextWaypoint].position;
        this.NavMesh.isStopped = false;
        anim.SetBool("isMoving", !NavMesh.isStopped);
        if (this.NavMesh.remainingDistance <= NavMesh.stoppingDistance && !this.NavMesh.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % patrolWayPoints.Length;
        }
    }

    public override void Walk()
    {

        waypointTimer += Time.deltaTime;

        float distanceBetweenWayPoint = (waypoint - selfTransform.position).sqrMagnitude;
        if (distanceBetweenWayPoint < (1 * 1) || isWaypointBlocked || waypointTimer > waypointTravelTime)
        {
            anim.SetBool("isMoving", false);
            anim.SetBool("isMovingSides", false);
            Wander();
        }
        else
        {
            anim.SetBool("isMoving", true);
            this.NavMesh.destination = waypoint;
            this.NavMesh.isStopped = false;
        }
        
    }

    public override void Chase()
    {
        if (!this.NavMesh.pathPending && (playerTransform.position - selfTransform.position).sqrMagnitude > minDistanceChase * minDistanceChase)
        {
            this.NavMesh.destination = playerTransform.position;
            this.NavMesh.isStopped = false;
            anim.SetBool("isMoving", true);
            
        }
        if (this.NavMesh.remainingDistance < minDistanceChase)
        {
           this.NavMesh.isStopped = true;
           anim.SetBool("isMoving", false);
           anim.SetBool("isMovingSides", false);
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
            Instantiate(corpse, selfTransform.position, selfTransform.rotation);
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
        Vector3 direction = playerTransform.position - selfTransform.position;
        direction = Quaternion.Euler(anguloDisparo, anguloDisparo, 0 ) * direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction = direction.normalized;

        bulletObj = Instantiate(bulletPrefab, selfTransform.position, Quaternion.AngleAxis(angle, Vector3.forward)) as GameObject;
        bulletObj.GetComponent<BulletEnemy>().Init(bulletDamage, direction, bulletSpeed);
    }

    // Create a new way point target
    private void Wander()
    {
        waypointTimer = 0;
        // does nothing except pick a new destination to go to
        waypoint = new Vector3(UnityEngine.Random.Range(selfTransform.position.x - walkRange, selfTransform.position.x + walkRange),
                               0, UnityEngine.Random.Range(selfTransform.position.z - walkRange, selfTransform.position.z + walkRange));

        // if the waypoint is blocked the Walk method will call Wander() again to make sure the enemy follows a correct path.
        isWaypointBlocked = Physics.Linecast(selfTransform.position, waypoint,raycastMask);

        float distance = (waypoint - selfTransform.position).sqrMagnitude;
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
