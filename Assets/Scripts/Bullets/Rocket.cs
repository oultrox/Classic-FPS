using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeMagnitude = 5f;
    private GameObject explosionPrefab;
    private LayerMask layerMask;
    private float radius;
    private int damage;
    private float lifeTime;
    private float lifeCounter;

   	void Start () {
        lifeCounter = 0;
	}

    public void Init(int _damage, float _radius, LayerMask _explosionLayer, GameObject _explosionPrefab)
    {
        this.damage = _damage;
        this.radius = _radius;
        this.layerMask = _explosionLayer;
        this.explosionPrefab = _explosionPrefab;
    }

    private void Explode()
    {
        Destroy(this.gameObject);
    }

    void Update ()
    {
        lifeTime += Time.deltaTime;
        if (lifeCounter >= lifeTime)
        {
            Explode();
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Collider[] hitColliders = Physics.OverlapSphere(contact.point, radius, layerMask);
        Instantiate(explosionPrefab, contact.point, Quaternion.identity);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.CompareTag("Enemy") == false)
            {
                continue;
            }
            hitColliders[i].GetComponent<Enemy>().TakeDamage(damage);
        }
        Destroy(this.gameObject);
        CameraShake.instance.StartShakeRotating(shakeDuration, shakeMagnitude);
    }

}
