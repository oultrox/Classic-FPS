using UnityEngine;

public class LimitFps : MonoBehaviour
{
    public int framerate; 
    private void Start()
    {
        Application.targetFrameRate = framerate;
    }
}