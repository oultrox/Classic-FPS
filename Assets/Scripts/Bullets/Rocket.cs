using UnityEngine;

public class Rocket : MonoBehaviour 
{
    [SerializeField] private float shakeDuration = 0.1f;
    [SerializeField] private float shakeMagnitude = 5f;
    private GameObject explosionPrefab;
    private LayerMask layerMask;
    private float radius;
    private int damage;
    private float lifeTime = 3f;
    private float lifeCounter;

    void Start () 
    {
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
        if (collision.gameObject.CompareTag("Player"))
        {
            return;
        }
        ContactPoint contact = collision.contacts[0];
        Explode(contact.point);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyController"))
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
            if (hitColliders[i].gameObject.CompareTag("Enemy")) 
            {
                hitColliders[i].GetComponent<EnemyController>().TakeDamage(damage);
            }
        }

        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
        ManagerShake.instance.StartShakeRotating(shakeDuration, shakeMagnitude);
    }
}
