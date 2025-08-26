using DumbInjector;
using FPSEngine.Source.DI.Containers;
using UnityEngine;
using UnityEngine.Serialization;

public class PlayerInstaller : MonoBehaviour, IDependencyProvider
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] CameraShaker cameraShaker;
    [SerializeField] DynamicCrosshair guiCrosshair;
    
    [SerializeField] GameObject rocketPrefab;
    [SerializeField] GameObject explosionPrefab;
    [SerializeField] GameObject spawnPoint;
    [SerializeField] Sprite idlePistol;
    [SerializeField] Sprite shotPistol;
    [SerializeField] GameObject bulletHolePrefab;
    
    [Provide]
    public WeaponContainer ProvideWeaponContainer()
    {
        return new WeaponContainer(rocketPrefab, explosionPrefab, spawnPoint, idlePistol, shotPistol, bulletHolePrefab);
    }
    
    [Provide]
    public  CameraShaker ProvideCameraShaker()
    {
        return cameraShaker;
    }
    
    [Provide]
    public  DynamicCrosshair ProvideDynamicCrosshair()
    {
        return guiCrosshair;
    }
    
    [Provide]
    public  IHasHealth ProvidePlayerHealth()
    {
        return playerHealth;
    }
    
}