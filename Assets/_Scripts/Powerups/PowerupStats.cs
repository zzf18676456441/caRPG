using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupStats : MonoBehaviour
{
    [Header("Stat Effects")]
    public float maxHealthAdd = 0;
    public float maxHealthMult = 0;
    public float maxArmorAdd = 0;
    public float maxArmorMult = 0;
    public float maxNO2Add = 0;
    public float maxNO2Mult = 0;
    public float weightAdd = 0;
    public float gripAdd = 0;
    public float gripMult = 0;
    public float accelerationAdd = 0;
    public float accelerationMult = 0;
    public float topSpeedAdd = 0;
    public float topSpeedMult = 0;

    private StatPack pack;
    void Start(){
        pack = new StatPack();
        pack.SetAdd(StatPack.StatType.Acceleration, accelerationAdd);
        pack.SetAdd(StatPack.StatType.Armor, maxArmorAdd);
        pack.SetAdd(StatPack.StatType.Grip, gripAdd);
        pack.SetAdd(StatPack.StatType.Health, maxHealthAdd);
        pack.SetAdd(StatPack.StatType.Nitro, maxNO2Add);
        pack.SetAdd(StatPack.StatType.TopSpeed, topSpeedAdd);
        pack.SetAdd(StatPack.StatType.Weight, weightAdd);
        pack.SetMult(StatPack.StatType.Acceleration, accelerationMult);
        pack.SetMult(StatPack.StatType.Armor, maxArmorMult);
        pack.SetMult(StatPack.StatType.Grip, gripMult);
        pack.SetMult(StatPack.StatType.Health, maxHealthMult);
        pack.SetMult(StatPack.StatType.Nitro, maxNO2Mult);
        pack.SetMult(StatPack.StatType.TopSpeed, topSpeedMult);
    }

    public StatPack GetPack(){
        return pack;
    }
}


public class StatPack{
    public enum StatType {Acceleration, Nitro, TopSpeed, Grip, Weight, Health, Armor};
    private Dictionary<StatType, float> Adds;
    private Dictionary<StatType, float> Mults;

    public StatPack(){
        Adds = new Dictionary<StatType, float>();
        Mults = new Dictionary<StatType, float>();
        foreach(StatType type in System.Enum.GetValues(typeof(StatType))){
            Adds[type] = 0f;
            Mults[type] = 0f;
        }
    }

    public void SetAdd(StatType type, float value){
        Adds[type] = value;
    }
    public void SetMult(StatType type, float value){
        Mults[type] = value;
    }

    public float GetAdd(StatType type){
        return Adds[type];
    }
    public float GetMult(StatType type){
        return Mults[type];
    }

}