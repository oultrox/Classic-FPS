using System.Collections;
using DumbInjector;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject prefabToSpawn; // Enemy, object, etc.
    [SerializeField] private float spawnInterval = 2f; // seconds between spawns
    [SerializeField] private GameObject sceneContext;   // assign the GameObject holding SceneContextInjector

    private IInjector _injector;

    private void Awake()
    {
        if (sceneContext != null)
        {
            // Grab the IInjector from the scene context
            _injector = sceneContext.GetComponent<DumbInjector.IInjector>();
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
            Instantiate(prefabToSpawn);
        }
    }
}
