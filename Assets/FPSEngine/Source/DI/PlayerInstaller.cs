using DumbInjector;
using UnityEngine;

public class PlayerInstaller : MonoBehaviour, IDependencyProvider
{
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] CameraShaker cameraShaker;
    [SerializeField] DynamicCrosshair guiCrosshair;
    
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