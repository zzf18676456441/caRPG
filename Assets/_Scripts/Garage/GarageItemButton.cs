using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GarageItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GarageSlider slider;
    public GameObject item;
    private bool noItem = false;
    private bool isWeapon = false;
    private WeaponMount weaponMount;
    private ModMount modMount;

    private AudioSource source;

    void Awake()
    {
        source = FindObjectOfType<Camera>().GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (noItem){
            if (isWeapon){
                slider.ShowUnequippedItem(weaponMount);
            } else {
                slider.ShowUnequippedItem(modMount);
            }
        } else { 
            slider.ShowPotentialItem(item);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slider.SetSlidersOnNewEquip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (noItem) return;  // Or play unequipping sound!
        if (item.GetComponent<PowerupMain>().IsOwned()){
            source.pitch = UnityEngine.Random.Range(.95f, 1.15f);
            source.Play();
        }
    }

    public void Unequipper(string name){
        noItem = true;
        switch(name){
            case "FrontWeapons":
                weaponMount = WeaponMount.Grill;
                isWeapon = true;
                break;
            case "RearWeapons":
                weaponMount = WeaponMount.Hitch;
                isWeapon = true;
                break;
            case "DoorWeapons":
                weaponMount = WeaponMount.Doors;
                isWeapon = true;
                break;
            case "RoofWeapons":
                weaponMount = WeaponMount.Roof;
                isWeapon = true;
                break;
            case "WheelWeapons":
                weaponMount = WeaponMount.Wheels;
                isWeapon = true;
                break;
            case "EngineMods":
                modMount = ModMount.Engine;
                isWeapon = false;
                break;
            case "NitroMods":
                modMount = ModMount.Trunk;
                isWeapon = false;
                break;
            case "ChassisMods":
                modMount = ModMount.Frame;
                isWeapon = false;
                break;
            case "BumperMods":
                modMount = ModMount.Bumpers;
                isWeapon = false;
                break;
            case "WheelMods":
                modMount = ModMount.Tires;
                isWeapon = false;
                break;       
        }
    }

    public void Unequip(GameController controller){
        if(isWeapon) controller.GetCar().GetComponent<PowerupManager>().Remove(weaponMount);
        else controller.GetCar().GetComponent<PowerupManager>().Remove(modMount);
    }
}
