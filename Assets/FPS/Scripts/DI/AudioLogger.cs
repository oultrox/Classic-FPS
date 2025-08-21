using UnityEngine;

namespace FPS.Scripts.DI
{
    public class AudioLogger : MonoBehaviour, IAudioHandler 
    {
        public void Log(string message)
        {
            Debug.Log("=========== AUDIO LOGGER: "  + message);
        }
    }
}