using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageMenuBar : MonoBehaviour
{
    public void UpdateButtons(List<GameObject> items){
        int index = 1;
        foreach(GameObject item in items){
            PowerupMain main = item.GetComponent<PowerupMain>();
            Transform element = gameObject.transform.Find("Inventory Area").Find("Items").Find("Upgrade " + index);
            element.GetChild(0).GetComponent<Text>().text = item.gameObject.name;
            element.GetComponent<Image>().sprite = item.GetComponent<SpriteRenderer>().sprite;
            if(main.IsOwned()){
                element.GetComponent<Button>().interactable = true;
                element.GetChild(0).GetComponent<Text>().color = Color.white;
                element.GetComponent<Image>().color = Color.white;
            } else {
                element.GetComponent<Button>().interactable = false;
                element.GetComponent<Image>().color = Color.black;
            }
            if(!main.IsChecked()){
                main.Check();
                StatPack stats = item.GetComponent<PowerupStats>().GetPack();
                if (stats != null){
                    SendStats(stats, item.GetComponent<PowerupAttachable>());
                }
            }
            ButtonScripts scripts = element.gameObject.AddComponent<ButtonScripts>();
            GarageItemButton gIB = element.gameObject.AddComponent<GarageItemButton>();
            scripts.SetItem(item);
            element.GetComponent<Button>().onClick.AddListener(scripts.EquipItem);
            GarageSlider slider = GameObject.Find("GarageUI").transform.Find("StatSliders").GetComponent<GarageSlider>();
            gIB.slider = slider;
            gIB.item = item;
            element.GetComponent<Button>().onClick.AddListener(slider.SetSlidersOnNewEquip);
            index++;
        }

        Transform ItemBar = gameObject.transform.Find("Inventory Area").Find("Items");
        ItemBar.GetComponent<RectTransform>().sizeDelta = new Vector2(66 * index, 62);

        for(;index<8;index++){
            Transform element = gameObject.transform.Find("Inventory Area").Find("Items").Find("Upgrade " + index);
            element.gameObject.SetActive(false);
        }
    }

    private void SendStats(StatPack stats, PowerupAttachable attachable){
        if(attachable.isWeapon) GarageStats.TryUpdate(stats, attachable.weaponLocation);
        else GarageStats.TryUpdate(stats, attachable.modLocation);
    }
}
