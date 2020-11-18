using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageController : MonoBehaviour
{
    GameController controller;
    InventoryMaster inventory;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
        inventory = controller.GetComponent<InventoryMaster>();
    }

    void Start()
    {
        UpdateEquips();    
    }



    private void UpdateEquips(){
        GameObject ItemGroup = transform.Find("Inventory").Find("Upgrades Tab").Find("ItemGroup").gameObject;
        foreach(GarageMenuBar child in ItemGroup.transform.GetComponentsInChildren<GarageMenuBar>())
        {
            switch (child.gameObject.name){
                case "Engine":
                    child.UpdateButtons(inventory.EngineMods);
                break;
                case "Wheels":
                    child.UpdateButtons(inventory.TireMods);
                break;
                case "Chassis":
                    child.UpdateButtons(inventory.BumperMods);
                break;
                case "Nitro":
                    child.UpdateButtons(inventory.TrunkMods);
                break;
                case "Exterior":
                    child.UpdateButtons(inventory.FrameMods);
                break;
            }
        }

        ItemGroup = transform.Find("Inventory").Find("Attachments Tab").Find("ItemGroup").gameObject;
        foreach(GarageMenuBar child in ItemGroup.transform.GetComponentsInChildren<GarageMenuBar>())
        {
            switch (child.gameObject.name){
                case "Front Bumper":
                    child.UpdateButtons(inventory.GrillWeapons);
                break;
                case "Rear Bumper":
                    child.UpdateButtons(inventory.HitchWeapons);
                break;
                case "Rims":
                    child.UpdateButtons(inventory.WheelWeapons);
                break;
                case "Doors":
                    child.UpdateButtons(inventory.DoorWeapons);
                break;
                case "Roof":
                    child.UpdateButtons(inventory.RoofWeapons);
                break;
            }
        }

    }
}
