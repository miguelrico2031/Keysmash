using UnityEngine.Audio;
using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;

    private void Awake()
    {
        foreach(Sound s in Sounds)
        {
            s.Source = gameObject.AddComponent<AudioSource>();
            s.Source.clip = s.Clip;
            s.Source.volume = s.Volume;
            s.Source.pitch = s.Pitch;
        }
    }

    public void PlaySound(string name)
    {
        foreach (Sound s in Sounds)
        {

            if (name == s.Name)
            {
                s.Source.Play();
            }
        }
    }
}
