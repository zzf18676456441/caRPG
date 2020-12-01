using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GarageItemButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public GarageSlider slider;
    public GameObject item;

    private AudioSource source;

    void Awake()
    {
        source = FindObjectOfType<Camera>().GetComponent<AudioSource>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Debug.Log("THIS IS HAPPENING!!!");
        slider.ShowPotentialItem(item);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        slider.SetSlidersOnNewEquip();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        source.pitch = UnityEngine.Random.Range(.95f, 1.15f);
        source.Play();
    }
}
