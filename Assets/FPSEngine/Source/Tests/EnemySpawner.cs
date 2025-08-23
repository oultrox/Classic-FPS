using System.Collections;
using DumbInjector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn; 
    [SerializeField] private float spawnInterval = 2f; 
    [SerializeField] private GameObject sceneContext;

    private IInjector _injector;

    private void Awake()
    {
        if (sceneContext != null)
        {
            // Grab the IInjector from the scene context
            _injector = sceneContext.GetComponent<IInjector>();
        }
    }

    private void Start()
    {
        if (prefabToSpawn != null)
        {
            StartCoroutine(SpawnTimerCoroutine());
        }
    }

    private IEnumerator SpawnTimerCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            var instance = Instantiate(prefabToSpawn);
            foreach (var mb in instance.GetComponents<MonoBehaviour>())
            {
                _injector.Inject(mb);
            }
        }
    }
}
