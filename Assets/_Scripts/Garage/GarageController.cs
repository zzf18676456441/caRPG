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
        sliderController = transform.Find("StatSliders").GetComponent<GarageSlider>();
        UpdateEquips();
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
    
    private static Dictionary<ModMount, StatPack> ModMin = new Dictionary<ModMount, StatPack>();
    private static Dictionary<ModMount, StatPack> ModMax = new Dictionary<ModMount, StatPack>();
    private static Dictionary<WeaponMount, StatPack> WeaponMin = new Dictionary<WeaponMount, StatPack>();
    private static Dictionary<WeaponMount, StatPack> WeaponMax = new Dictionary<WeaponMount, StatPack>();

    public static StatPack baseStats = new StatPack();
    public static StatPack currentStats = new StatPack();

    private static StatPack minStats = new StatPack();
    private static StatPack maxStats = new StatPack();
    
    public static void TryUpdate(StatPack pack, ModMount mod){
       if(ModMin.ContainsKey(mod)){
            ModMin[mod] = StatPack.Min(ModMin[mod],pack);
            ModMax[mod] = StatPack.Max(ModMax[mod],pack);
       } else {
            ModMin[mod] = StatPack.Min(new StatPack(),pack);
            ModMax[mod] = StatPack.Max(new StatPack(),pack);
       }
       UpdateMinMax();
    }

    public static void TryUpdate(StatPack pack, WeaponMount weapon){
        if(WeaponMin.ContainsKey(weapon)){
            WeaponMin[weapon] = StatPack.Min(WeaponMin[weapon],pack);
            WeaponMax[weapon] = StatPack.Max(WeaponMax[weapon],pack);
       } else {
            WeaponMin[weapon] = StatPack.Min(new StatPack(),pack);
            WeaponMax[weapon] = StatPack.Max(new StatPack(),pack);
       }
       UpdateMinMax();
    }

    private static void UpdateMinMax(){
        minStats = new StatPack();
        maxStats = new StatPack();
        foreach(StatPack pack in ModMin.Values){
            minStats += pack;
        }
        foreach(StatPack pack in ModMax.Values){
            maxStats += pack;
        }

        foreach(StatPack pack in WeaponMin.Values){
            minStats += pack;
        }
        foreach(StatPack pack in WeaponMax.Values){
            maxStats += pack;
        }
    }


    public static float MinStatValue(StatPack.StatType stat){
        return (baseStats.GetAdd(stat) + minStats.GetAdd(stat)) * (1 + minStats.GetMult(stat));
    }

    public static float MaxStatValue(StatPack.StatType stat){
        return (baseStats.GetAdd(stat) + maxStats.GetAdd(stat)) * (1 + maxStats.GetMult(stat));
    }

    public static void SetBaseStats(StatPack pack){
        baseStats = pack;
    }

    public static void SetCurrentStats(StatPack pack){
        currentStats = pack;
    }

    public static float CurrentStatValue(StatPack.StatType type){
        return currentStats.GetAdd(type);
    }

}
