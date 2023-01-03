using System.Collections;
using UnityEngine;

// based on http://unitytipsandtricks.blogspot.com/2013/05/camera-shake.html
public class ManagerShake : MonoBehaviour
{
    public static ManagerShake instance;
    [Header("Shake properties")]
    [SerializeField] private float speed = 20f;
    [SerializeField] private AnimationCurve damper = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.9f, .33f, -2f, -2f), new Keyframe(1f, 0f, -5.65f, -5.65f));

    [Header("Zooming juice")]
    [SerializeField] private float zoomSpeed = 20f;
    [SerializeField] private float maxZoomFOV = 80f;

    private float shakeIntensity = 1;
    private Transform originalTransform;
    private Quaternion originalRot;
    private Camera cameraSelf;
    private float originalFOV;

    private void Awake()
    {
        instance = this;
        originalTransform = this.transform;
        cameraSelf = this.GetComponent<Camera>();
        originalFOV = cameraSelf.fieldOfView;
    }

    /// <summary>
    /// Shakes the camera rotating it's transform.
    /// </summary>
    public void StartShakeRotating(float _duration, float _magnitude)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRotation(_duration, _magnitude * shakeIntensity, damper));
        
    }

    public void StartZoomEffect()
    {
        StartCoroutine(ZoomIn());
    }

    IEnumerator ShakeRotation(float duration, float magnitude, AnimationCurve damper = null)
    {
        Vector3 originalEuler;
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (ManagerScreen.instance.IsPaused())
            {
                break;
            }

            originalRot = originalTransform.localRotation;
            originalEuler = originalRot.eulerAngles;
            elapsed += Time.deltaTime;
            float damperedMag = (damper != null) ? (damper.Evaluate(elapsed / duration) * magnitude) : magnitude;
            float x = (Mathf.PerlinNoise(Time.time * speed, 0f) * damperedMag) - (damperedMag / 2f);
            float y = (Mathf.PerlinNoise(0f, Time.time * speed) * damperedMag) - (damperedMag / 2f);
            float z = (Mathf.PerlinNoise(0.5f, Time.time * speed * 0.5f) * damperedMag) - (damperedMag / 2f);
            originalTransform.localRotation = Quaternion.Euler(new Vector3(originalEuler.x + x, originalEuler.y + y, originalEuler.z + z));
            yield return null;
        }
        originalTransform.localRotation = originalRot;
    }


    IEnumerator ZoomIn()
    {
        bool  isFinished = false;
        bool isZoomingIn = true;
        while (isFinished == false)
        {
            if ( cameraSelf.fieldOfView >= maxZoomFOV)
            {
                isZoomingIn = false;
            }
            else
            {
                if (isZoomingIn == false && cameraSelf.fieldOfView <= originalFOV)
                {
                    isFinished = true;
                }
            }

            if (isZoomingIn)
            {
                cameraSelf.fieldOfView += (zoomSpeed / 8) * Time.deltaTime;
            }
            else
            {
                cameraSelf.fieldOfView -= (zoomSpeed / 8) * Time.deltaTime;
            }
            yield return null;
        }
        cameraSelf.fieldOfView = originalFOV;
    }

    #region Properties
    public float ShakeIntensity
    {
        get
        {
            return shakeIntensity;
        }

        set
        {
            shakeIntensity = value;
        }
    }
    #endregion  

}