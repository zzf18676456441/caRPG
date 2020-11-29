using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goreSoundHandler : MonoBehaviour
{
    public List<AudioClip> goreSounds;
    public AudioSource source;
    void Start()
    {
        source.clip = goreSounds[Random.Range(0, goreSounds.Count)];
        source.Play();
    }
}
