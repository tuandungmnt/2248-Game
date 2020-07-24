using System;
using Data;
using UnityEngine;

namespace Domain
{
    public class AudioManager : MonoBehaviour
    {
        public SoundData[] sounds;

        private void Start() 
        {
            DontDestroyOnLoad(gameObject);

            foreach (var s in sounds) 
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clip;

                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
            }
            Play("Theme");
        }

        public void Play(string soundName) 
        {
            var s = Array.Find(sounds, sound => sound.name == soundName);
            if (s == null) {
                //Debug.Log("Sound not found");
                return;
            }
        
            s.source.Play();
        }
    }
}
