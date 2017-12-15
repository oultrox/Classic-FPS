using System;
using System.Collections;
using UnityEngine;

public class BulletEnemy : MonoBehaviour {

    [SerializeField] private float destroyTime = 3;
    private Rigidbody rgBody;
    private int damage = 35;

    //-------- Metodos API -------------
    //Consigue los elementos físicos
    private void Awake()
    {
        rgBody = this.GetComponent<Rigidbody>();
        StartCoroutine(DestroyByTimer());
    }

    //Condiciona la colisión en caso de ser el player o ser otra bala.
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            col.gameObject.SendMessage("TakeDamage",damage, SendMessageOptions.DontRequireReceiver);
            Destroy(this.gameObject);
        }
        else if (col.gameObject.CompareTag("Wall"))
        {
            Destroy(this.gameObject);
        }
    }

    //--------- Métodos custom ---------------

    public void Init(int bulletDamage, Vector3 direction, float speed)
    {
        damage = bulletDamage;
        rgBody.velocity = direction * speed;
    }
    //Tiempo de destruccion de la bala.
    IEnumerator DestroyByTimer()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}