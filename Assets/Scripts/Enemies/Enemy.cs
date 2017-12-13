using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour {

    [SerializeField] protected int enemyHealth = 100;

    public abstract bool IsLooking();
    public abstract void Walk();
    public abstract void Chase();
    public abstract void Attack();
    public abstract void TakeDamage(int damage);
    public abstract void Die();

}
