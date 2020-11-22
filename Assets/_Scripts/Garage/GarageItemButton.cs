using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GarageItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GarageSlider slider;
    public GameObject item;

    public void OnPointerEnter(PointerEventData eventData){
        Debug.Log("THIS IS HAPPENING!!!");
        slider.ShowPotentialItem(item);
    }

    public void OnPointerExit(PointerEventData eventData){
        slider.SetSlidersOnNewEquip();
    }

}
