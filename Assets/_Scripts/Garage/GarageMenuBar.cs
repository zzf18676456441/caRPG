using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageMenuBar : MonoBehaviour
{
    public void UpdateButtons(List<GameObject> items){
        int index = 1;
        Transform element = transform.Find("Slot0");
        Select(element.gameObject);
        PowerupMain main;
        
        GarageItemButton gIB = element.gameObject.AddComponent<GarageItemButton>();
        ButtonScripts scripts = element.gameObject.GetComponent<ButtonScripts>();
        GarageSlider slider = GameObject.Find("GarageUI").transform.Find("StatSliders").GetComponent<GarageSlider>();
        gIB.slider = slider;
        gIB.Unequipper(gameObject.name);
        element.GetComponent<Button>().onClick.AddListener(scripts.UnequipItem);

        foreach(GameObject item in items){
            main = item.GetComponent<PowerupMain>();
            element = gameObject.transform.Find("Slot" + index);
            element.Find("Text").GetComponent<Text>().text = item.gameObject.name;
            element.Find("Image").GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
            if(main.IsOwned()){
                element.Find("Image").GetComponent<Image>().color = Color.white;
            } else {
                element.Find("Image").GetComponent<Image>().color = Color.black;
            }
            if(!main.IsChecked()){
                main.Check();
                StatPack stats = item.GetComponent<PowerupStats>().GetPack();
                if (stats != null){
                    SendStats(stats, item.GetComponent<PowerupAttachable>());
                }
            }
            if(main.IsEquipped()){
                Select(element.gameObject);
            }
            scripts = element.gameObject.GetComponent<ButtonScripts>();
            gIB = element.gameObject.AddComponent<GarageItemButton>();
            scripts.SetItem(item);
            element.GetComponent<Button>().onClick.AddListener(scripts.EquipItem);
            slider = GameObject.Find("GarageUI").transform.Find("StatSliders").GetComponent<GarageSlider>();
            gIB.slider = slider;
            gIB.item = item;
            element.GetComponent<Button>().onClick.AddListener(slider.SetSlidersOnNewEquip);
            index++;
        }

        for(;index<5;index++){
            element = gameObject.transform.Find("Slot" + index);
            element.gameObject.SetActive(false);
        }
    }

    public void Select(GameObject button){
        foreach(Button child in gameObject.GetComponentsInChildren<Button>()){
            if (child.name == button.name){child.interactable = false;}
            else child.interactable = true;
        }
    }

    private void SendStats(StatPack stats, PowerupAttachable attachable){
        if(attachable.isWeapon) GarageStats.TryUpdate(stats, attachable.weaponLocation, attachable.baseDamage);
        else GarageStats.TryUpdate(stats, attachable.modLocation);
    }
}
