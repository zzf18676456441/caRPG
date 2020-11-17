using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScripts : MonoBehaviour
{
   GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    public void StartNextLevel(){
        controller.StartNextLevel();
    }

    public void RetryLevel(){
        controller.RetryLevel();
    }

    public void StartGarageLevel(){
        controller.StartGarageLevel();
    }
}
