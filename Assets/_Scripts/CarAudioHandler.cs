using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarAudioHandler : MonoBehaviour
{
    public AudioSource source;
    private Rigidbody2D rbody;
    private float baseVolume;
    private float basePitch;
    void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        baseVolume = source.volume;
        basePitch = source.pitch;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        source.pitch = Mathf.Clamp(basePitch * .05f * rbody.velocity.magnitude, 0, 2.65f);
    }
}
