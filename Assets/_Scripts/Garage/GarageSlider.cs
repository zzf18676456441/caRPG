using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageSlider : MonoBehaviour
{
    GameController controller;
    // Start is called before the first frame update
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    public void SetSlidersOnOpen(){
        Slider slider;
        StatPack.StatType type;
        type = StatPack.StatType.Acceleration;
        slider = PickSlider(type);
        slider.value = sliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.Armor;
        slider = PickSlider(type);
        slider.value = sliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.Grip;
        slider = PickSlider(type);
        slider.value = sliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.Health;
        slider = PickSlider(type);
        slider.value = sliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.Nitro;
        slider = PickSlider(type);
        slider.value = sliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.TopSpeed;
        slider = PickSlider(type);
        slider.value = sliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.Weight;
        slider = PickSlider(type);
        slider.value = sliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
    }


    private float sliderPosition(float min, float max, float value){
        if (max == min) return 0.5f;
        return 0.8f * (value - min) / (max - min) + 0.1f;
    }

    private Slider PickSlider(StatPack.StatType type){
        switch (type){
            case StatPack.StatType.Acceleration:
            return transform.Find("Acceleration Group").Find("AccelerationSlider").GetComponent<Slider>();
            case StatPack.StatType.Armor:
            return transform.Find("Armor Group").Find("Armor Slider").GetComponent<Slider>();
            //case StatPack.StatType.Damage:
            //return transform.Find("Damage Group").Find("Damage Slider").GetComponent<Slider>();
            case StatPack.StatType.Grip:
            return transform.Find("Grip Group").Find("Grip Slider").GetComponent<Slider>();
            case StatPack.StatType.Health:
            return transform.Find("Health Group").Find("Health Slider").GetComponent<Slider>();
            case StatPack.StatType.Nitro:
            return transform.Find("Nitro Group").Find("NitroSlider").GetComponent<Slider>();
            case StatPack.StatType.TopSpeed:
            return transform.Find("Top Speed Group").Find("Top Speed Slider").GetComponent<Slider>();
            case StatPack.StatType.Weight:
            return transform.Find("Weight Group").Find("Weight Slider").GetComponent<Slider>();
            default:
            return null;            
        }
    }
}
