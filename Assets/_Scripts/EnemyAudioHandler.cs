using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioHandler : MonoBehaviour
{
    public List<AudioClip> actionSounds, deathSounds;
    public AudioSource source;
    public float actionSoundTimer = 2.5f;
    private float actionSoundTime;
    void Awake()
    {
        actionSoundTime = 0;
    }
    public void PlayActionSound()
    {
        if (actionSoundTime <= 0)
        {
            source.clip = actionSounds[Random.Range(0, actionSounds.Count)];
            source.Play();
            source.pitch = Random.Range(0.85f, 1f);
            actionSoundTime = actionSoundTimer;
        }
        else
        {
            if (Random.Range(0, 2) == 0)
                actionSoundTime -= Time.deltaTime;
        }
    }
    public void PlayDeathSound()
    {
        source.Stop();
        source.clip = deathSounds[Random.Range(0, deathSounds.Count)];
        source.Play();
    }
}
