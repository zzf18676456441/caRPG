using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarEffects : MonoBehaviour
{
    public TrailRenderer NitroL;
    public TrailRenderer NitroR;
    public float previousNO2;
    GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
        previousNO2 = controller.GetPlayer().currentNO2;
    }
    // Update is called once per frame
    void Update()
    {
        if (previousNO2 > controller.GetPlayer().currentNO2 && !NitroL.emitting && !NitroR.emitting)
        {
            NitroR.emitting = true;
            NitroL.emitting = true;
        }
        else if (previousNO2 == controller.GetPlayer().currentNO2)
        {
            NitroR.emitting = false;
            NitroL.emitting = false;
        }
        else
        {
            previousNO2 = controller.GetPlayer().currentNO2;
        }

    }
}
