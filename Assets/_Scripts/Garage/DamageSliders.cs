using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DamageSliders : MonoBehaviour
{
    GameController controller;

    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    public void SetSliders(){
        float damage = 0f;
        PowerupManager car = controller.GetCar().GetComponent<PowerupManager>();
        Slider slider = transform.Find("FrontSlider").GetComponent<Slider>();
        damage = car.GetWeaponDamage(WeaponMount.Grill);
        slider.value = GarageSlider.SliderPosition(GarageStats.minDamage,GarageStats.maxDamage,damage);
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);
        
        slider = transform.Find("RearSlider").GetComponent<Slider>();
        damage = car.GetWeaponDamage(WeaponMount.Hitch);
        slider.value = GarageSlider.SliderPosition(GarageStats.minDamage,GarageStats.maxDamage,damage);
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);

        slider = transform.Find("RoofSlider").GetComponent<Slider>();
        damage = car.GetWeaponDamage(WeaponMount.Roof);
        slider.value = GarageSlider.SliderPosition(GarageStats.minDamage,GarageStats.maxDamage,damage);
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);

        slider = transform.Find("DoorSlider").GetComponent<Slider>();
        damage = car.GetWeaponDamage(WeaponMount.Doors);
        slider.value = GarageSlider.SliderPosition(GarageStats.minDamage,GarageStats.maxDamage,damage);
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);

        slider = transform.Find("WheelSlider").GetComponent<Slider>();
        damage = car.GetWeaponDamage(WeaponMount.Wheels);
        slider.value = GarageSlider.SliderPosition(GarageStats.minDamage,GarageStats.maxDamage,damage);
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);
    }

    public void ShowPotentialWeapon(PowerupAttachable weapon){
        ShowPotentialWeapon(weapon.weaponLocation, weapon.baseDamage);
    }


    public void ShowPotentialWeapon(WeaponMount weaponLocation, float damage){
        Slider slider;
        PowerupManager car = controller.GetCar().GetComponent<PowerupManager>();
        switch(weaponLocation){
            case WeaponMount.Grill:
                slider = transform.Find("FrontSlider").GetComponent<Slider>();
                break;
            case WeaponMount.Hitch:
                slider = transform.Find("RearSlider").GetComponent<Slider>();
                break;
            case WeaponMount.Roof:
                slider = transform.Find("RoofSlider").GetComponent<Slider>();
                break;
            case WeaponMount.Doors:
                slider = transform.Find("DoorSlider").GetComponent<Slider>();
                break;
            default:
                slider = transform.Find("WheelSlider").GetComponent<Slider>();
                break;
        }

        float newPosition = GarageSlider.SliderPosition(GarageStats.minDamage, GarageStats.maxDamage, damage);
        if (newPosition > slider.value + 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0,1,0,0.3f);
        }
        if (newPosition < slider.value + 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,0,0.3f);
        }
        slider.value = newPosition;
      
    }

}
