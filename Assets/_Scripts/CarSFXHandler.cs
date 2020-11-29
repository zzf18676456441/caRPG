using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSFXHandler : MonoBehaviour
{
    public List<AudioClip> tireSqueals, carCrashes;
    public AudioSource audioPlayer;
    public void PlaySqueal()
    {
        audioPlayer.clip = tireSqueals[UnityEngine.Random.Range(0, tireSqueals.Count)];
        audioPlayer.Play();
    }
}
