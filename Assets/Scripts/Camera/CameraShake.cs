using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{

    // How long the object should shake for.
    public static float shakeDuration = 0f;

    // Amplitude of the shake. A larger value shakes the camera harder.
    public  float shakeAmount = 0.7f;
    public  float decreaseFactor = 1.0f;

    // Transform of the camera to shake. Grabs the gameObject's transform
    // if null.
    private Transform camTransform;
    private Vector3 originalPos;
    private bool isActive = false;

    void Awake()
    {
        if (camTransform == null)
        {
            camTransform = GetComponent(typeof(Transform)) as Transform;
        }
    }

    void OnEnable()
    {
        originalPos = camTransform.localPosition;
    }

    void Update()
    {
        if (shakeDuration > 0)
        {
            camTransform.localPosition = originalPos + Random.insideUnitSphere * shakeAmount;

            shakeDuration -= Time.deltaTime * decreaseFactor;
            isActive = true;
        }
        else if(isActive)
        {
            shakeDuration = 0f;
            camTransform.localPosition = originalPos;
            isActive = false;
        }
    }
}