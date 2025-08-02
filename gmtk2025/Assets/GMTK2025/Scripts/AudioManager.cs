using System;
using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public Sound[] Sounds;
    public static AudioManager instance;
    private bool isWalking;
    private Coroutine footStepCoroutine;

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
        Sound sound = Array.Find(Sounds, s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning("Sound not found: " + name);
            return;
        }
        if (!IsPlaying(name)) sound.source.Play();
    }

    private bool IsPlaying(string name)
    {
        Sound sound = Array.Find(Sounds, s => s.name == name);
        if (sound == null) return false;
        return sound.source.isPlaying;
    }

    public void StartFootSteps()
    {
        if (!isWalking)
        {
            isWalking = true;
            footStepCoroutine = StartCoroutine(FootStepLoop());
        }
    }

    public void StopFootSteps()
    {
        if (isWalking)
        {
            isWalking = false;
            if (footStepCoroutine != null)
            {
                StopCoroutine(footStepCoroutine);
            }

        }
    }

    private IEnumerator FootStepLoop()
    {
        string[] footsteps = { "FootStep1", "FootStep2", "FootStep3" };

        while (isWalking)
        {
            string chosenSound = footsteps[UnityEngine.Random.Range(0, 3)];
            PlaySound(chosenSound);

            Sound stepSound = Array.Find(Sounds, s => s.name == chosenSound);
            float waitTime = stepSound.clip.length;

            yield return new WaitForSeconds(waitTime);
        
        }
    }
}
