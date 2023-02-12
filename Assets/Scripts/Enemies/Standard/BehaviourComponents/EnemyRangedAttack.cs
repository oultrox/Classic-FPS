using System;
using System.Collections;
using UnityEngine;


public class EnemyRangedAttack : MonoBehaviour,IEnemyAttack
{
    [Header("Attack")]
    [SerializeField] private float attackRate = 1;

    [Header("Proyectil")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletDamage = 20;
    [SerializeField] private float bulletSpeed = 10;
    
    // TODO: Move to the health component.
    [SerializeField] private int enemyHP = 100;

    private GameObject bulletObj;
    private Transform playerTransform;
    private float attackTimer;

    private void Awake()
    {
        playerTransform = PlayerHealth.instance.GetComponent<Transform>();
    }
    public void Init()
    {

    }

    public void Tick()
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

    private void Shoot(float anguloDisparo)
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction = Quaternion.Euler(anguloDisparo, anguloDisparo, 0) * direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction = direction.normalized;

        bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward)) as GameObject;
        bulletObj.GetComponent<BulletEnemy>().Init(bulletDamage, direction, bulletSpeed);
    }
}
