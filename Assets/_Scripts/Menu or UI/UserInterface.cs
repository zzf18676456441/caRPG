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

    GameController controller;
    void Awake(){
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        speed = 2.23694f * controller.GetCar().GetComponent<Rigidbody2D>().velocity.magnitude;
        hp = controller.GetPlayer().currentHealth / controller.GetPlayer().maxHealth;
        no2 = controller.GetPlayer().currentNO2 / controller.GetPlayer().maxNO2;

        speedometer.transform.rotation = Quaternion.Euler(speedometer.transform.rotation.eulerAngles.x, speedometer.transform.rotation.eulerAngles.y, Mathf.Max(38 - (speed * 141f/120f), -103));

        Image healthbar = health.GetComponent<Image>();
        healthbar.fillAmount = hp;
        healthbar.color = new Color(1f*(1-hp), 1f*hp, 0f);

        Image no2bar = nitrous.GetComponent<Image>();
        no2bar.fillAmount = no2;

    }

    public void DrawSign(TurnType turnType)
    {

    }

    public void HideSign()
    {

    }
}
