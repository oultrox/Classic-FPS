using UnityEngine;
using UnityEngine.Audio;

public class ManagerAudio : MonoBehaviour {

    public static ManagerAudio instance = null;
    [SerializeField] private AudioMixer masterMixer;
    [SerializeField] private Sound[] sounds;

    //Inicializar el hecho de que este objeto persista sobre las escenas para evitar que se reinicie la canción.
    void Awake ()
    {
        //Singleton
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
            
        //Carga de los elementos del sonido asda    
        foreach (Sound s in sounds)
        {
            //Carga el componente de audio source que contiene cada elemento del array con los campos de la clase "Sound", 
            //ese campo está declarado en la clase que se utiliza como tipo de dato "Sound".
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.outputAudioMixerGroup = s.AudioMixer;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
            s.Source.playOnAwake = s.IsPlayedOnAwake;
            s.Source.loop = s.IsLoopeable;
        }
        DontDestroyOnLoad(this.gameObject);
        System.GC.Collect();
    }

    //Método que busca el nombre de la canción en el arreglo de el AudioManager.
    public void Play(string name)
    {
        Sound s = null;
        for (int i = 0; i < sounds.Length; i++)
        {
            if (name == sounds[i].Name)
            {
                s = sounds[i];
                break;
            }
        }
        //Si la encontró, reproducir.
        if (s !=null)
        {
            s.Source.Play();
        }else
        {
            Debug.LogWarning("Audio no encontrado.");
        }
    }

    //Properties - Getters y setters para los mixers para ser llamados por el PauseGUI manager y sus sliders.
    public AudioMixer MasterMixer
    {
        get
        {
            return masterMixer;
        }
        set
        {
            masterMixer = value;
        }
    }
}
