using UnityEngine;

public class ManagerScreen : MonoBehaviour
{
    public enum Screen
    {
        Title,
        Settings,
        Tutorial,
        Game,
        Pause,
        GameOver,
        Restarting
    };

    public enum Scenes
    {
        Intro = 0,
        Title = 1,
        Instructions = 2,
        Main = 3
    }

    public static ManagerScreen instance;
    public static bool debugON = false;
    private Screen actualScreen;

    //-------Métodos API-------
    void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    //-------Metodos custom-------
    public bool IsPausable()
    {
        if (actualScreen == Screen.GameOver ||
            actualScreen == Screen.Restarting ||
            actualScreen == Screen.Pause ||
            actualScreen == Screen.Settings)
        {
            return false;
        }
        return true;
    }

    public bool IsPaused()
    {
        if (actualScreen == Screen.Pause ||
            actualScreen == Screen.Settings)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Log(string message)
    {
        if (debugON)
        {
            Debug.Log("###: " + message);
        }
    }

    #region Properties
    public Screen ActualScreen
    {
        get
        {
            return actualScreen;
        }

        set
        {
            Log("Pantalla actual: " + actualScreen + " | Pantalla nueva: " + value);
            actualScreen = value;
        }
    }
    #endregion

}
