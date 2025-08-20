using Enemies.Standard.InterfaceComponents;
using UnityEngine;

[DisallowMultipleComponent]
public class EnemyRangedAttack : MonoBehaviour, IEnemyAttack
{
    [Header("Attack")]
    [SerializeField] private float attackRate = 1;

    [Header("Proyectil")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private int bulletDamage = 20;
    [SerializeField] private float bulletSpeed = 10;
    
    private GameObject bulletObj;
    private Transform playerTransform;
    private float attackTimer;
    
    public void Tick()
    {
        attackTimer += Time.deltaTime;
        if (attackTimer < attackRate)
            return;

        attackTimer = 0f;
        Shoot(0);
    }

    public void InjectTarget(Transform target)
    {
        playerTransform = target;
    }

    /// <summary>
    /// Enemy just instantiates a prefab directly to the target.
    /// </summary>
    /// <param name="anguloDisparo"></param>
    private void Shoot(float anguloDisparo)
    {
        Vector3 direction = playerTransform.position - transform.position;
        direction = Quaternion.Euler(anguloDisparo, anguloDisparo, 0) * direction;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        direction = direction.normalized;

        bulletObj = Instantiate(bulletPrefab, transform.position, Quaternion.AngleAxis(angle, Vector3.forward));
        bulletObj.GetComponent<BulletEnemy>().Init(bulletDamage, direction, bulletSpeed);
    }
}
