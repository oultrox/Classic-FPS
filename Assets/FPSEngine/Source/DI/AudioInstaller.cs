using DumbInjector;
using UnityEngine;

namespace FPS.Scripts.DI
{
    public class AudioInstaller : MonoBehaviour, IDependencyProvider
    {
        [SerializeField] AudioLogger Audio;

        [Provide]
        IAudioHandler ProvideAudio()
        {
            return Audio;
        }
    }
}