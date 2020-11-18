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
            ButtonScripts scripts = element.gameObject.AddComponent<ButtonScripts>();
            scripts.SetItem(item);
            element.GetComponent<Button>().onClick.AddListener(scripts.EquipItem);
            index++;
        }

        Transform ItemBar = gameObject.transform.Find("Inventory Area").Find("Items");
        ItemBar.GetComponent<RectTransform>().sizeDelta = new Vector2(66 * index, 62);

        for(;index<8;index++){
            Transform element = gameObject.transform.Find("Inventory Area").Find("Items").Find("Upgrade " + index);
            element.gameObject.SetActive(false);
        }
    }
}
