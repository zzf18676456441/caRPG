using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioHandler : MonoBehaviour
{
    public AudioSource audio;
    private Rigidbody2D rigidbody;
    private float baseVolume;
    private float basePitch;
    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        baseVolume = audio.volume;
        basePitch = audio.pitch;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        audio.pitch = Mathf.Clamp(basePitch * .05f * rigidbody.velocity.magnitude, 0, 2.65f);
    }
}
