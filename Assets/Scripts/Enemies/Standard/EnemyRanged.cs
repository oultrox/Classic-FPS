using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{
    [Header("Proyectil")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletDamage = 20;
    [SerializeField] private float bulletSpeed = 10;

    [Header("Attack")]
    [SerializeField] private float attackRate = 1;
    [SerializeField] private float sightDistance = 10f;

    [Header("Patrol")]
    [SerializeField] private Transform[] patrolWayPoints;

    private GameObject bulletObj;
    private Transform playerTransform;
    private float attackTimer;
    private int nextWaypoint = 0;

    public override void Awake()
    {
        base.Awake();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override bool IsLooking()
    {
        return (playerTransform.position - enemyTransform.position).sqrMagnitude < sightDistance * sightDistance;
    }

    public override void Walk()
    {
        
        navMesh.destination = patrolWayPoints[nextWaypoint].position;
        navMesh.isStopped = false;
        if (navMesh.remainingDistance <= navMesh.stoppingDistance && !navMesh.pathPending)
        {
            nextWaypoint = (nextWaypoint + 1) % patrolWayPoints.Length;
        }
        animator.SetBool("isMoving", true);
    }

    public override void Chase()
    {
        if (!navMesh.pathPending && (playerTransform.position - enemyTransform.position).sqrMagnitude > minDistanceChase * minDistanceChase)
        {
            navMesh.destination = playerTransform.position;
            navMesh.isStopped = false;
            animator.SetBool("isMoving", true);
        }
        if (navMesh.remainingDistance < minDistanceChase)
        {
           navMesh.isStopped = true;
            animator.SetBool("isMoving", false);
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

    private IEnumerator Alert()
    {
        sightDistance *= 4;
        yield return new WaitForSeconds(0.1f);
        sightDistance /= 4;
    }

    public override void Die()
    {
        //Checking for testing purposes.
        if (corpse)
        {
            Instantiate(corpse, enemyTransform.position, enemyTransform.rotation);
        }
        Destroy(gameObject);
    }

    //------------------------------------------------------------------
    // Private methods that helps the Enemy implementation
    //------------------------------------------------------------------

    private void Shoot(float anguloDisparo)
    {
        Vector3 direction = playerTransform.position - enemyTransform.position;
        direction = Quaternion.Euler(anguloDisparo, anguloDisparo, 0 ) * direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction = direction.normalized;

        bulletObj = Instantiate(bulletPrefab, enemyTransform.position, Quaternion.AngleAxis(angle, Vector3.forward)) as GameObject;
        bulletObj.GetComponent<BulletEnemy>().Init(bulletDamage, direction, bulletSpeed);
    }
}
