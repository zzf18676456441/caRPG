using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInterface : MonoBehaviour
{
    public static float speed = 0;

    public static float hp = 100;

    public static float no2 = 100;

    public static int points = 0;

    public GameObject speedometer;

    public GameObject health;

    public GameObject nitrous;

    public GameObject score;


    GameController controller;
    void Awake(){
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = 2.23694f * controller.GetCar().GetComponent<Rigidbody2D>().velocity.magnitude;
        hp = controller.GetPlayer().currentHealth / controller.GetPlayer().maxHealth;

        Text speedt = speedometer.GetComponent<Text>();

        speedt.text = string.Format("Speed: {0:0.} mph", speed);

        Text healtht = health.GetComponent<Text>();

        healtht.text = string.Format("Health: {0:0.}%", hp);

        Text nitroust = nitrous.GetComponent<Text>();

        nitroust.text = "NO\u2082: " + no2 + "%";

        Text scoret = score.GetComponent<Text>();

        scoret.text = "Score: " + no2;
    }
}
