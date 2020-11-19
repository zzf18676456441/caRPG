using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFX : MonoBehaviour
{
    public Camera mainCam;
    public Animation crashAnim;
    private float prevHealth;
    GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
        prevHealth = controller.GetPlayer().currentHealth;
    }
    void FixedUpdate()
    {
        mainCam.orthographicSize = 27 + .15f * (controller.GetCar().GetComponent<Rigidbody2D>().velocity.magnitude);
        if (prevHealth > controller.GetPlayer().currentHealth)
        {
            crashAnim.Play();
        }
        prevHealth = controller.GetPlayer().currentHealth;
    }
}
