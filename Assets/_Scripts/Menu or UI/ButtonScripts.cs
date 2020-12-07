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
        if(attachedItem.GetComponent<PowerupMain>().IsOwned()){
            controller.GetCar().GetComponent<PowerupManager>().Attach(attachedItem.GetComponent<PowerupAttachable>(), AttachType.Fixed);
            transform.parent.GetComponent<GarageMenuBar>().Select(this.gameObject);
        }
    }

    public void UnequipItem(){
        GarageItemButton gIB = gameObject.GetComponent<GarageItemButton>();
        gIB.Unequip(controller);
        transform.parent.GetComponent<GarageMenuBar>().Select(this.gameObject);
    }

    public void Quit(){
        controller.ExitGame();
    }

    public void MainMenu(){
        controller.MainMenu();
    }

    public void ShowPopup(GameObject popup){
        // Logic to ensure multiple level selects aren't opened
        if (popup.TryGetComponent<LevelSelectOpen>(out LevelSelectOpen lco))
        {
            if (LevelSelectOpen.open)
                return;
            else
                LevelSelectOpen.open = true;
        }

        // Creates a popup
        GameObject shown = Instantiate(popup, transform.parent);
    }

    public void StartButtonPresetLevel(){
        controller.StartLevel(number);
    }

    public void KillParent(){
        Destroy(gameObject.transform.parent.gameObject);
    }

}
