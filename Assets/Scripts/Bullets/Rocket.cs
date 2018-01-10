using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour {

    [SerializeField] private GameObject explosionPrefab;

    [SerializeField] private float radius;
    [SerializeField] private int damage;
    [SerializeField] private float lifeTime;
    [SerializeField] private LayerMask layerMask;

    private float lifeCounter;

	void Start () {
        lifeCounter = 0;
	}
	
	void Update ()
    {
        lifeTime += Time.deltaTime;
        if (lifeCounter >= lifeTime)
        {
            Explode();
        }
	}

    private void Explode()
    {
        Destroy(this.gameObject);
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
    }
}
