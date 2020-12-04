using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScripts : MonoBehaviour
{
    GameController controller;
    private GameObject attachedItem;

    public int number;

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

    public void SetItem(GameObject item){
        attachedItem = item;
    }

    public void EquipItem(){
        controller.GetCar().GetComponent<PowerupManager>().Attach(attachedItem.GetComponent<PowerupAttachable>(), AttachType.Fixed);
    }

    public void Quit(){
        controller.ExitGame();
    }

    public void MainMenu(){
        controller.MainMenu();
    }

    public void ShowPopup(GameObject popup){
        GameObject shown = Instantiate(popup,transform.parent);
    }

    public void StartButtonPresetLevel(){
        controller.StartLevel(number);
    }

    public void KillParent(){
        Destroy(gameObject.transform.parent.gameObject);
    }

}
