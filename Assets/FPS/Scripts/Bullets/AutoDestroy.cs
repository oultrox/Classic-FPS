using UnityEngine;

public class AutoDestroy : MonoBehaviour{

    [SerializeField] private float lifeTime = 3f;

    private void Awake()
    {
        Destroy(gameObject,lifeTime);
    }
}
