using UnityEngine;
using System.Collections;

// based on http://unitytipsandtricks.blogspot.com/2013/05/camera-shake.html
public class CameraShake : MonoBehaviour
{
    public static CameraShake instance;
    [SerializeField] private float speed = 20f;
    [SerializeField] private AnimationCurve damper = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.9f, .33f, -2f, -2f), new Keyframe(1f, 0f, -5.65f, -5.65f));

    private Transform originalTransform;
    private Vector3 originalPos;
    private Quaternion originalRot;

    private void Awake()
    {
        instance = this;
        originalTransform = this.transform;
    }

    /// <summary>
    /// Shakes the camera rotating it's transform.
    /// </summary>
    public void StartShakeRotating(float _duration, float _magnitude)
    {
        StopAllCoroutines();
        StartCoroutine(ShakeRotation(_duration, _magnitude, damper));
    }

    IEnumerator ShakeRotation(float duration, float magnitude, AnimationCurve damper = null)
    {
        Vector3 originalEuler;
        float elapsed = 0f;
        while (elapsed < duration)
        {
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

}