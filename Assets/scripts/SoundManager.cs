using UnityEngine;
using System.Collections.Generic;
using System.Linq;
    public class SoundManager : MonoBehaviour 
    {
                        //Drag a reference to the audio source which will play the music.
        public static SoundManager _instance = null;        //Allows other scripts to call functions from SoundManager.                
        public List<AudioSource> sources;        //The highest a sound effect will be randomly pitched.


        void Awake ()
        {
            //Check if there is already an _instance of SoundManager
            if (_instance == null)
                //if not, set it to this.
                _instance = this;
            //If _instance already exists:
            else if (_instance != this)
                //Destroy this, this enforces our singleton pattern so there can only be one _instance of SoundManager.
                Destroy (gameObject);

            //Set SoundManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
            DontDestroyOnLoad (gameObject);
        }


        //Used to play single sound clips.
        public void PlaySingle(AudioClip clip)
        {
            //Set the clip of our efxSource audio source to the clip passed in as a parameter.
    
        }
        public void DoAudio (bool value)
        {
            sources=FindObjectsOfType<AudioSource>().ToList();
            sources.ForEach(s=>{
                s.mute=value;
            });
            
           
        }
    }