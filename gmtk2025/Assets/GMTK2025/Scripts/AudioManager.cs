using System;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    public static AudioManager instance;

    void Awake()
    {
        foreach (Sound sound in Sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loops;
        }
        instance = this;
    }

    void Start()
    {
        PlaySound("AmbientCaveLoop");
    }

    public void PlaySound(string name)
    {
        Sound sound = Array.Find(Sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }
        sound.source.Play();
    }
}
