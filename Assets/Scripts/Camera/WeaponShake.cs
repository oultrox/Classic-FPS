using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponShake : MonoBehaviour {

    public static WeaponShake instance;
    [SerializeField] private float speed = 20f;
    [SerializeField] private AnimationCurve damper = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(0.9f, .33f, -2f, -2f), new Keyframe(1f, 0f, -5.65f, -5.65f));

    private Transform originalTransform;
    private Vector3 originalPos;

    private void Awake()
    {
        instance = this;
        originalTransform = this.transform;
    }

    /// <summary>
    /// Shakes the camera moving it's transform's position.
    /// </summary>
    public void StartShake(float _duration, float _magnitude)
    {
        StopAllCoroutines();
        originalPos = originalTransform.localPosition;
        StartCoroutine(ShakePosition(_duration, _magnitude, damper));
    }

    IEnumerator ShakePosition(float duration, float magnitude, AnimationCurve damper = null)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            if (ManagerScreen.instance.IsPaused())
            {
                break;
            }

            elapsed += Time.deltaTime;
            float damperedMag = (damper != null) ? (damper.Evaluate(elapsed / duration) * magnitude) : magnitude;
            float x = (Mathf.PerlinNoise(Time.time * speed, 0f) * damperedMag) - (damperedMag / 2f);
            float y = (Mathf.PerlinNoise(0f, Time.time * speed) * damperedMag) - (damperedMag / 2f);
            transform.localPosition = new Vector3(originalPos.x + x, originalPos.y + y, originalPos.z);
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
