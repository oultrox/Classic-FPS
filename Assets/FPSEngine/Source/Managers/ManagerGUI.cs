using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerGUI : MonoBehaviour {

    public static ManagerGUI instance;
    [SerializeField] private Image hurtScreen;
    private float hurtInitialAlpha;
    private Coroutine hurtCoroutine;

    //Singleton creation
    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    // Use this for initialization
    private void Start ()
    {
        hurtInitialAlpha = hurtScreen.color.a; 
    }

    public void HurtBlink()
    {
        hurtScreen.enabled = true;
        if (hurtCoroutine != null)
        {
            StopCoroutine(hurtCoroutine);
        }
        hurtCoroutine = StartCoroutine(HurtBlink(hurtScreen, 0.03f));
    }

    /// <summary> Hurt screen blink corroutine. </summary>
    /// <param name="image"> Image to be faded.</param>
    /// <param name="speed"> transition speed, the less, the faster.</param>
    public IEnumerator HurtBlink(Image image, float speed)
    {
        Color colorImage;
        colorImage = image.color;
        colorImage.a = hurtInitialAlpha;
        image.color = colorImage;
        WaitForSeconds delay = new WaitForSeconds(speed);
        for (int i = 0; i < 20; i++)
        { 
            yield return delay;
            colorImage.a -= 0.05f;
            image.color = colorImage;
        }
    }

    /// <summary> Fade out corroutine. </summary>
    /// <param name="image"> Image to be faded.</param>
    /// <param name="speed"> transition speed, the less, the faster.</param>
    public IEnumerator FadeOut(Image image, float speed)
    {
        Color colorImage;
        colorImage = image.color;
        colorImage.a = hurtInitialAlpha;
        image.color = colorImage;
        WaitForSeconds delay = new WaitForSeconds(speed);
        for (int i = 0; i < 20; i++)
        {
            yield return delay;
            colorImage.a -= 0.05f;
            image.color = colorImage;
        }
    }

}
