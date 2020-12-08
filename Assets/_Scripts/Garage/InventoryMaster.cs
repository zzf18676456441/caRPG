using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryMaster : MonoBehaviour
{
    public List<GameObject> BumperMods;
    public List<GameObject> EngineMods;
    public List<GameObject> FrameMods;
    public List<GameObject> TireMods;
    public List<GameObject> TrunkMods;
    public List<GameObject> GrillWeapons;
    public List<GameObject> HitchWeapons;
    public List<GameObject> RoofWeapons;
    public List<GameObject> DoorWeapons;
    public List<GameObject> WheelWeapons; 

    public List<List<GameObject>> AllItems;

    void Awake(){
        AllItems = new List<List<GameObject>>();
        AllItems.Add(BumperMods);
        AllItems.Add(EngineMods);
        AllItems.Add(FrameMods);
        AllItems.Add(TireMods);
        AllItems.Add(TrunkMods);
        AllItems.Add(GrillWeapons);
        AllItems.Add(HitchWeapons);
        AllItems.Add(RoofWeapons);
        AllItems.Add(DoorWeapons);
        AllItems.Add(WheelWeapons);
    } 
}
