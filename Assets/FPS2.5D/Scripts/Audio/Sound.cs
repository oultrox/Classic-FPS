using UnityEngine.Audio;
using UnityEngine;

//Clase que es utilizada como tipo de dato para el arreglo del audioManager.
[System.Serializable]
public class Sound {

    [Range(0f,1f)]
    [SerializeField] private float volume;

    [Range(.1f, 3f)]
    [SerializeField] private float pitch;
    
    [SerializeField] private string name;
    [SerializeField] private bool isPlayedOnAwake;
    [SerializeField] private bool isLoopeable;
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioMixerGroup audioMixer;
    private AudioSource source;

    #region Properties
        public string Name
        {
            get
            {
                return name;
            }

            set
            {
                name = value;
            }
        }

        public float Volume
        {
            get
            {
                return volume;
            }

            set
            {
                volume = value;
            }
        }

        public float Pitch
        {
            get
            {
                return pitch;
            }

            set
            {
                pitch = value;
            }
        }

        public bool IsPlayedOnAwake
        {
            get
            {
                return isPlayedOnAwake;
            }

            set
            {
                isPlayedOnAwake = value;
            }
        }

        public AudioClip Clip
        {
            get
            {
                return clip;
            }

            set
            {
                clip = value;
            }
        }

        public AudioSource Source
        {
            get
            {
                return source;
            }

            set
            {
                source = value;
            }
        }

        public bool IsLoopeable
        {
            get
            {
                return isLoopeable;
            }

            set
            {
                isLoopeable = value;
            }
        }

        public AudioMixerGroup AudioMixer
        {
            get
            {
                return audioMixer;
            }

            set
            {
                audioMixer = value;
            }
        }   
    #endregion
}
