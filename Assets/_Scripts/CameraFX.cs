using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFX : MonoBehaviour
{
    public Camera mainCam;
    GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    void FixedUpdate()
    {
        mainCam.orthographicSize = 27 + .15f * (controller.GetCar().GetComponent<Rigidbody2D>().velocity.magnitude);
    }
}
