using System;
using UnityEngine;
using UnityEngine.UI;

public class GUIPause : MonoBehaviour {

    public static GUIPause instance;

    [SerializeField] private Text optMotionText;
    [SerializeField] private Text optBloomText;
    [SerializeField] private Text optVignetteText;
    [SerializeField] private Text optChromaticText;
    [SerializeField] private Text optParticlesText;
    [SerializeField] private Text optFullsScreenText;
    [SerializeField] private Slider shakenessBar;
    [SerializeField] private Slider mainVolumeBar;
    [SerializeField] private Texture2D cursorTexture;
    private CursorMode cursorMode = CursorMode.ForceSoftware;

    //----------Metodos API----------
    // Creación del singleton para poder ser accedida a la instancia por otras clases.
    private void Awake()
    {
        //Singleton creation
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    public void Start()
    {
        this.Init();
        
    }

    //--------Metodos Custom--------------
    //Inicialización
    private void Init()
    {
        //Inicialización de los sliders.
        float initialMainVolumeLevel;
        ManagerAudio.instance.MasterMixer.GetFloat("masterVolume", out initialMainVolumeLevel);
        mainVolumeBar.value = initialMainVolumeLevel;
        shakenessBar.value = CameraShake.instance.ShakeIntensity;

        //Adds a listener to the main slider and invokes a method when the value changes.
        shakenessBar.onValueChanged.AddListener (delegate { ValueShakenessChangeCheck(); });
        mainVolumeBar.onValueChanged.AddListener(delegate { ValueVolumeChangeCheck(); });
    }
    
    // Invoked when the value of the slider changes.
    public void ValueShakenessChangeCheck()
    {
        CameraShake.instance.ShakeIntensity = shakenessBar.value;
    }

    public void ValueVolumeChangeCheck()
    {
        ManagerAudio.instance.MasterMixer.SetFloat("masterVolume", mainVolumeBar.value);
        if (mainVolumeBar.value <= -40)
        {
            ManagerAudio.instance.MasterMixer.SetFloat("masterVolume", -80);
        }
    }

    //Setea el nuevo cursor al mouse.
    public void EnablePauseCursor()
    {
        Vector2 hotSpot = new Vector2(cursorTexture.width / 2 , cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        Cursor.visible = true;
    }
    public void DisablePauseCursor()
    {
        Cursor.visible = false;
    }

    #region Properties
    public Text OptMotionText
    {
        get
        {
            return optMotionText;
        }

        set
        {
            optMotionText = value;
        }
    }

    public Text OptBloomText
    {
        get
        {
            return optBloomText;
        }

        set
        {
            optBloomText = value;
        }
    }

    public Text OptVignetteText
    {
        get
        {
            return optVignetteText;
        }

        set
        {
            optVignetteText = value;
        }
    }

    public Text OptChromaticText
    {
        get
        {
            return optChromaticText;
        }

        set
        {
            optChromaticText = value;
        }
    }

    public Text OptParticlesText
    {
        get
        {
            return optParticlesText;
        }

        set
        {
            optParticlesText = value;
        }
    }

    public Slider ShakenessBar
    {
        get
        {
            return shakenessBar;
        }
        set
        {
            shakenessBar = value;
        }
    }

    public Text OptFullsScreenText
    {
        get
        {
            return optFullsScreenText;
        }
        set
        {
            optFullsScreenText = value;
        }
    }

    public Slider MainVolumeBar
    {
        get
        {
            return mainVolumeBar;
        }

        set
        {
            mainVolumeBar = value;
        }
    }
    #endregion
}

