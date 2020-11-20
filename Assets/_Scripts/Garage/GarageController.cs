using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarageController : MonoBehaviour
{
    GameController controller;
    InventoryMaster inventory;
    GarageSlider sliderController;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
        inventory = controller.GetComponent<InventoryMaster>();
    }

    void Start()
    {
        UpdateEquips();
        sliderController = transform.Find("StatSliders").GetComponent<GarageSlider>();
        sliderController.SetSlidersOnOpen();
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



public static class GarageStats{
    
    private static Dictionary<StatPack.StatType, float> maxStatValsBase = new Dictionary<StatPack.StatType, float>();
    private static Dictionary<StatPack.StatType, float> maxStatValsMult = new Dictionary<StatPack.StatType, float>();
    private static Dictionary<StatPack.StatType, float> minStatValsBase = new Dictionary<StatPack.StatType, float>();
    private static Dictionary<StatPack.StatType, float> minStatValsMult = new Dictionary<StatPack.StatType, float>();
    
    private static StatPack baseStats = new StatPack();
    private static StatPack currentStats = new StatPack();


    public static void TryUpdate(StatPack pack){
       //TODO:  How does this work now?
    }

    public static float MinStatValue(StatPack.StatType stat){
        return (baseStats.GetAdd(stat) + minStatValsBase[stat]) * (1 + minStatValsMult[stat]);
    }

    public static float MaxStatValue(StatPack.StatType stat){
        return (baseStats.GetAdd(stat) + maxStatValsBase[stat]) * (1 + maxStatValsMult[stat]);
    }

    public static void SetBaseStats(StatPack pack){
        baseStats = pack;
        //Weight never gets multiplied
        maxStatValsMult[StatPack.StatType.Weight] = 0; 
        minStatValsMult[StatPack.StatType.Weight] = 0;
    }

    public static void SetCurrentStats(StatPack pack){
        currentStats = pack;
    }

    public static float CurrentStatValue(StatPack.StatType type){
        return currentStats.GetAdd(type);
    }

}
