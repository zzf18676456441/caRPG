using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    int i = 0;
    GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<SinglePointMovement>().LookAt(new Vector2(10f, 0f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //i++;
        //if (i == 100) GetComponent<SinglePointMovement>().MoveToLocation(new Vector2 (0f, 20f));
        //Debug.Log(GetComponent<SinglePointMovement>().GetAngle());
    }
}
