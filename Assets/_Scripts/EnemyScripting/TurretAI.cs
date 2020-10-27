using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{

    GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SinglePointMovement>().LookAt(controller.GetCar().transform);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //Debug.Log(GetComponent<SinglePointMovement>().GetAngle());
    }
}
