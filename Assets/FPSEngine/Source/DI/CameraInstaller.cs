using DumbInjector;
using UnityEngine;

public class CameraInstaller : MonoBehaviour, IDependencyProvider
{
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
    
}