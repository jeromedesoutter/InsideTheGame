using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Sound
{
    public string name;
    public AudioSource source;
};

[System.Serializable]
public class AudioManager
{

    public Sound[] sounds;

    public void PlaySound(string key)
    {
       foreach(Sound s in sounds)
        {
            if (s.name.Equals(key))
            {
                s.source.Play();
            }

        }
    }
    
}
