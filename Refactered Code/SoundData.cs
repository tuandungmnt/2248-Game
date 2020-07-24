using UnityEngine;

namespace Data
{
    [System.Serializable]
    public class SoundData
    {
        public string name;
        public AudioClip clip;

        public float volume;
        public float pitch;
        public bool loop;

        [HideInInspector]
        public AudioSource source;
    }
}
