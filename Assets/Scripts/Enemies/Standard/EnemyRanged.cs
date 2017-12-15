using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRanged : Enemy
{

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletDamage;
    [SerializeField] private float bulletSpeed;
    [SerializeField] private float shootRange;
    [SerializeField] private float attackRate = 1;

    private GameObject bulletObj;
    private Transform playerTransform;
    private float attackTimer;

    public override void Awake()
    {
        base.Awake();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    public override bool IsLooking()
    {
        return false;
    }

    public override void Walk()
    {

    }

    public override void Chase()
    {

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
    }

    public override void Die()
    {
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
