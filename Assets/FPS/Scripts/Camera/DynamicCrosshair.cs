using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DynamicCrosshair : MonoBehaviour {

    public static DynamicCrosshair instance;
    [SerializeField] private GameObject crosshairImage;
    [SerializeField] private Vector3 sizeDecrease = new Vector3(0.1f,0.1f,0);
    private RectTransform crosshairTransform;
    private Vector3 initialScale;
    private float expansionTimer;
    private float expansionAmount;

    private void Awake()
    {
        // Singleton creation
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        initialScale = crosshairImage.GetComponent<RectTransform>().localScale;
        crosshairTransform = crosshairImage.GetComponent<RectTransform>();
    }

    void Update ()
    {
        // Spread system

        // Expansion system
        if (expansionTimer >= 0 && Time.timeScale > 0)
        {
            crosshairTransform.localScale += sizeDecrease;
            expansionTimer -= Time.deltaTime;
        }
        else
        {
            if (crosshairTransform.localScale != initialScale)
            {
                crosshairTransform.localScale = initialScale;
            }
        }
    }

    #region Properties
    public float ExpansionTimer
    {
        get
        {
            return expansionTimer;
        }

        set
        {
            expansionTimer = value;
        }
    }

    public float ExpansionAmount
    {
        get
        {
            return expansionAmount;
        }

        set
        {
            expansionAmount = value;
        }
    }
    #endregion
}
