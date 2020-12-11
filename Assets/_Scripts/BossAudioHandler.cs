using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAudioHandler : MonoBehaviour
{
    public AudioClip firingSound;
    public List<AudioClip> actionSounds, deathSounds;
    public AudioSource source;
    public float actionSoundTimer = 12f;
    public float actionSoundTime;
    void Awake()
    {
        actionSoundTime = 0;
    }

    public void PlayActionSound()
    {
        if (actionSoundTime <= 0 && !source.isPlaying)
        {

            try
            {
                source.clip = actionSounds[UnityEngine.Random.Range(0, actionSounds.Count)];
            }
            catch (Exception) { }
            source.Play();
            source.pitch = UnityEngine.Random.Range(0.85f, 1f);
            actionSoundTime = actionSoundTimer;
        }
        else
        {
            if (UnityEngine.Random.Range(0, 2) == 0)
                actionSoundTime -= Time.deltaTime;
        }
    }

    public void PlayDeathSound()
    {
        source.Stop();
        try
        {
            source.clip = deathSounds[UnityEngine.Random.Range(0, deathSounds.Count)];
        }
        catch (Exception) { }
        source.Play();
    }

    public void PlayFiringSound()
    {
        source.Stop();
        source.clip = firingSound;
        source.Play();
    }
}
