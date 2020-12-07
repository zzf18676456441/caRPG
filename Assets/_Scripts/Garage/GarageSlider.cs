using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarageSlider : MonoBehaviour
{
    GameController controller;

    DamageSliders damageSliders;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
        damageSliders = transform.parent.GetComponentInChildren<DamageSliders>();
    }

    public void SetSlidersOnOpen(){
        Slider slider;
        StatPack.StatType type;
        type = StatPack.StatType.Acceleration;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.Armor;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.Grip;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.Health;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.Nitro;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.TopSpeed;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        type = StatPack.StatType.Weight;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));

        damageSliders.SetSliders();
    }



    public void SetSlidersOnNewEquip(){
        Slider slider;
        StatPack.StatType type;
        type = StatPack.StatType.Acceleration;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);
        type = StatPack.StatType.Armor;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);
        type = StatPack.StatType.Grip;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);
        type = StatPack.StatType.Health;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);
        type = StatPack.StatType.Nitro;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);
        type = StatPack.StatType.TopSpeed;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);
        type = StatPack.StatType.Weight;
        slider = PickSlider(type);
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), GarageStats.CurrentStatValue(type));
        slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,1,0.3f);
        
        damageSliders.SetSliders();
    }


    public void ShowPotentialItem(GameObject item){
        PowerupMain main = item.GetComponent<PowerupMain>();
        PowerupAttachable attach = item.GetComponent<PowerupAttachable>();
        StatPack pack = item.GetComponent<PowerupStats>().GetPack();
        PowerupAttachable otherItem;
        StatPack otherPack;
        if (main.IsEquipped()) return;
        if (attach.isWeapon){
            otherItem = controller.GetCar().GetComponent<PowerupManager>().GetWeapon(attach.weaponLocation);
            damageSliders.ShowPotentialWeapon(attach);
        }
        else
            otherItem = controller.GetCar().GetComponent<PowerupManager>().GetMod(attach.modLocation);
        if (otherItem == null){
            otherPack = new StatPack();
        } else {
            otherPack = otherItem.GetComponent<PowerupStats>().GetPack();
        }

        ShowStatBars(pack, otherPack);
    }

    public void ShowUnequippedItem(WeaponMount mount){
        StatPack pack = new StatPack();
        PowerupAttachable otherItem = controller.GetCar().GetComponent<PowerupManager>().GetWeapon(mount);
        if (otherItem == null) return;
        StatPack otherPack = otherItem.GetComponent<PowerupStats>().GetPack();

        ShowStatBars(pack, otherPack);
        damageSliders.ShowPotentialWeapon(mount, controller.GetCar().GetComponent<PowerupManager>().baseDamage);
    }

    public void ShowUnequippedItem(ModMount mount){
        StatPack pack = new StatPack();
        PowerupAttachable otherItem = controller.GetCar().GetComponent<PowerupManager>().GetMod(mount);
        if (otherItem == null) return;
        StatPack otherPack = otherItem.GetComponent<PowerupStats>().GetPack();

        ShowStatBars(pack, otherPack);
    }

    public void ShowStatBars(StatPack pack, StatPack otherPack){
        Slider slider;
        StatPack newStats;
        StatPack.StatType type;
        StatPack currentStats = GarageStats.currentStats;

        newStats = StatPack.Instead(GarageStats.currentStats,otherPack,pack);

        type = StatPack.StatType.Acceleration;
        slider = PickSlider(type);
        if (newStats.GetAdd(type) > currentStats.GetAdd(type) + 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0,1,0,0.3f);
        }
        if (newStats.GetAdd(type) < currentStats.GetAdd(type) - 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,0,0.3f);
        }
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), newStats.GetAdd(type));
        
        type = StatPack.StatType.Armor;
        slider = PickSlider(type);
        if (newStats.GetAdd(type) > currentStats.GetAdd(type) + 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0,1,0,0.3f);
        }
        if (newStats.GetAdd(type) < currentStats.GetAdd(type) - 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,0,0.3f);
        }
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), newStats.GetAdd(type));
        
        type = StatPack.StatType.Grip;
        slider = PickSlider(type);
        if (newStats.GetAdd(type) > currentStats.GetAdd(type) + 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0,1,0,0.3f);
        }
        if (newStats.GetAdd(type) < currentStats.GetAdd(type) - 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,0,0.3f);
        }
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), newStats.GetAdd(type));
        
        type = StatPack.StatType.Health;
        slider = PickSlider(type);
        if (newStats.GetAdd(type) > currentStats.GetAdd(type) + 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0,1,0,0.3f);
        }
        if (newStats.GetAdd(type) < currentStats.GetAdd(type) - 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,0,0.3f);
        }
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), newStats.GetAdd(type));
        
        type = StatPack.StatType.Nitro;
        slider = PickSlider(type);
        if (newStats.GetAdd(type) > currentStats.GetAdd(type) + 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0,1,0,0.3f);
        }
        if (newStats.GetAdd(type) < currentStats.GetAdd(type) - 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,0,0.3f);
        }
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), newStats.GetAdd(type));
        
        type = StatPack.StatType.TopSpeed;
        slider = PickSlider(type);
        if (newStats.GetAdd(type) > currentStats.GetAdd(type) + 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0,1,0,0.3f);
        }
        if (newStats.GetAdd(type) < currentStats.GetAdd(type) - 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,0,0.3f);
        }
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), newStats.GetAdd(type));
        
        type = StatPack.StatType.Weight;
        slider = PickSlider(type);
        if (newStats.GetAdd(type) > currentStats.GetAdd(type) + 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(0,1,0,0.3f);
        }
        if (newStats.GetAdd(type) < currentStats.GetAdd(type) - 0.00001f){
            slider.transform.Find("Fill Area").Find("Fill").GetComponent<Image>().color = new Color(1,0,0,0.3f);
        }
        slider.value = SliderPosition(GarageStats.MinStatValue(type), GarageStats.MaxStatValue(type), newStats.GetAdd(type));
        
    }

    public static float SliderPosition(float min, float max, float value){
        if (max == min) return 0.5f;
        return 0.9f * (value - min) / (max - min) + 0.1f;
    }

    private Slider PickSlider(StatPack.StatType type){
        switch (type){
            case StatPack.StatType.Acceleration:
            return transform.Find("AccelerationSlider").GetComponent<Slider>();
            case StatPack.StatType.Armor:
            return transform.Find("ArmorSlider").GetComponent<Slider>();
            case StatPack.StatType.Grip:
            return transform.Find("GripSlider").GetComponent<Slider>();
            case StatPack.StatType.Health:
            return transform.Find("HealthSlider").GetComponent<Slider>();
            case StatPack.StatType.Nitro:
            return transform.Find("NitroSlider").GetComponent<Slider>();
            case StatPack.StatType.TopSpeed:
            return transform.Find("TopSpeedSlider").GetComponent<Slider>();
            case StatPack.StatType.Weight:
            return transform.Find("WeightSlider").GetComponent<Slider>();
            default:
            return null;            
        }
    }
}
