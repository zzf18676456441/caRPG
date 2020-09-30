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


    // Update is called once per frame
    void Update()
    {
        Text speedt = speedometer.GetComponent<Text>();

        speedt.text = "Speed: " + speed + " mph";

        Text healtht = health.GetComponent<Text>();

        healtht.text = "Health: " + hp + "%";

        Text nitroust = nitrous.GetComponent<Text>();

        nitroust.text = "NO\u2082: " + no2 + "%";

        Text scoret = score.GetComponent<Text>();

        scoret.text = "Score: " + no2;
    }
}
