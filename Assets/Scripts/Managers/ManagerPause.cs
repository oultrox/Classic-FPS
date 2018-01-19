using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class ManagerPause : MonoBehaviour {

    private enum PostSetting
    {
        chromaAberration = 0,
        bloom = 1,
        motionBlur = 2,
        ColorGrading = 3,
        AmbientOcclusion = 4
    }

    public static ManagerPause instance;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private PostProcessProfile pp;

    private bool isMotionBlurEnabled;
    private bool isBloomEnabled;
    private bool isChromaticEnabled;
    private bool isVignetteEnabled;
    private bool isParticleEnabled;
    private ManagerScreen.Screen originalScreen;

    //----Métodos API------
    // Singleton creation
    private void Awake()
    {
        //Singleton creation
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
    }

    void Start ()
    {
        InicializarSettings();
    }

    void Update()
    {
        //Si oprime ESC o P y está en una pantalla pausable, pausará.
        if ((Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P)))
        {
            if (ManagerScreen.instance.IsPausable())
            {
                ActivarPause();
            }
            else if(ManagerScreen.instance.ActualScreen == ManagerScreen.Screen.Pause)
            {
                DesactivarPause();
            }
        }
    }

    //-------Métodos Custom-------
    //Inicializa los textos y las variables a base de los componentes activados en el menú de opciones.
    private void InicializarSettings()
    {
        //Activa el in-game crosshair.
        if (DynamicCrosshair.instance)
        {
            Cursor.visible = false;
        }
        
        //Operador ternario y asignación booleana directa para inicializar los settings del juego.
        isMotionBlurEnabled = pp.settings[(int)PostSetting.motionBlur].enabled;
        GUIPause.instance.OptMotionText.text = (isMotionBlurEnabled ? "MOTION BLUR: ON" : "MOTION BLUR: OFF");

        isChromaticEnabled = pp.settings[(int)PostSetting.chromaAberration].enabled;
        GUIPause.instance.OptChromaticText.text = (isChromaticEnabled ? "CHROMATIC ABERRATION: ON" : "CHROMATIC ABERRATION: OFF");

        isBloomEnabled = pp.settings[(int)PostSetting.bloom].enabled;
        GUIPause.instance.OptBloomText.text = isBloomEnabled ? "BLOOM: ON" : "BLOOM: OFF";

        System.GC.Collect();
    }

    //Pause toggle thingie
    public void ActivarPause()
    {
        originalScreen = ManagerScreen.instance.ActualScreen;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
        //GUIPause.instance.EnablePauseCursor();
        ManagerScreen.instance.ActualScreen = ManagerScreen.Screen.Pause;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        System.GC.Collect();  
    }

    //Also need of lock / unlock mouse aim.
    public void DesactivarPause()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        GUIPause.instance.DisablePauseCursor();
        Cursor.lockState = CursorLockMode.Locked;
        ManagerScreen.instance.ActualScreen = originalScreen;
    }

    public void ToggleOptionsMenu()
    {
        if (ManagerScreen.instance.ActualScreen == ManagerScreen.Screen.Pause)
        {
            ManagerScreen.instance.ActualScreen = ManagerScreen.Screen.Settings;
        }
        else
        {
            ManagerScreen.instance.ActualScreen = ManagerScreen.Screen.Pause;
        }
    }

    //In case of the need -> ragequit
    public void SalirJuego()
    {
        Application.Quit();
    }
     
    //Toggles de efectos en el menú de opciones que se utilizan a través de los triggers en el canvas con el editor de Unity.
    public void ToggleMotionBlur()
    {
        if (isMotionBlurEnabled)
        {
            GUIPause.instance.OptMotionText.text = "MOTION BLUR: OFF";
            isMotionBlurEnabled = pp.settings[(int)PostSetting.motionBlur].active = false;
        }
        else
        {
            GUIPause.instance.OptMotionText.text = "MOTION BLUR: ON";
            isMotionBlurEnabled = pp.settings[(int)PostSetting.motionBlur].active = true;
        }
    }

    public void ToggleChromaticAberration()
    {
        if (isChromaticEnabled)
        {
            GUIPause.instance.OptChromaticText.text = "CHROMATIC ABERRATION: OFF";
            isChromaticEnabled = pp.settings[(int)PostSetting.chromaAberration].active = false;
        }
        else
        {
            GUIPause.instance.OptChromaticText.text = "CHROMATIC ABERRATION: ON";
            isChromaticEnabled = pp.settings[(int)PostSetting.chromaAberration].active = true;
        }
    }

    public void ToggleBloom()
    {
        if (isBloomEnabled)
        {
            GUIPause.instance.OptBloomText.text = "BLOOM: OFF";
            isBloomEnabled = pp.settings[(int)PostSetting.bloom].active = false;
        }
        else
        {
            GUIPause.instance.OptBloomText.text = "BLOOM: ON";
            isBloomEnabled = pp.settings[(int)PostSetting.bloom].active = true;
        }
    }

    public void ToggleFullscreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }

    //Reinicia los ajustes a través de el event trigger UI con el editor de Unity.
    public void RestartSettings()
    {

        isMotionBlurEnabled = false;
        ToggleMotionBlur();

        isBloomEnabled = false;
        ToggleBloom();

        isChromaticEnabled = false;
        ToggleChromaticAberration();

        //El pauseGUI manager maneja el valor del a variable estática a través de su slider con el método ValueChangedCheck(). 
        GUIPause.instance.ShakenessBar.value = 1;
        GUIPause.instance.MainVolumeBar.value = 0;
        ManagerAudio.instance.MasterMixer.SetFloat("masterVolume", GUIPause.instance.MainVolumeBar.value);
    }
}
