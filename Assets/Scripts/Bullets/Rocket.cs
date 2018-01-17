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
    private float lifeTime = 3f;
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

    void Update ()
    {
        lifeCounter += Time.deltaTime;
        if (lifeCounter >= lifeTime)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnCollisionEnter(Collision collision)
    {
        ContactPoint contact = collision.contacts[0];
        Explode(contact.point);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Explode(other.transform.position);
        }
    }

    private void Explode(Vector3 contactPoint)
    {
        //Perhaps just using transform.position here could fix any future bug related to the positions for triggers.
        Collider[] hitColliders = Physics.OverlapSphere(contactPoint, radius, layerMask);
        for (int i = 0; i < hitColliders.Length; i++)
        {
            if (hitColliders[i].gameObject.CompareTag("Enemy") == false)
            {
                continue;
            }
            hitColliders[i].GetComponent<Enemy>().TakeDamage(damage);
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        CameraShake.instance.StartShakeRotating(shakeDuration, shakeMagnitude);
    }
}
