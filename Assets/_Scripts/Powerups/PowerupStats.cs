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

    private StatPack pack = new StatPack();

    public StatPack GetPack(){
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

    public static StatPack operator +(StatPack a, StatPack b){
        foreach(StatType type in System.Enum.GetValues(typeof(StatType))){
            a.SetAdd(type, a.GetAdd(type) + b.GetAdd(type));
            a.SetMult(type, a.GetMult(type) + b.GetMult(type));
        }
        return a;
    }

    public static StatPack ApplyToBase(StatPack baseStats, StatPack powerups){
        StatPack result = new StatPack();
        foreach(StatType type in System.Enum.GetValues(typeof(StatType))){
            result.SetAdd(type, (baseStats.GetAdd(type) + powerups.GetAdd(type)) * (1 + powerups.GetMult(type)));
        }
        return result;
    }

    public static StatPack Max(StatPack a, StatPack b){
        StatPack c = new StatPack();
        foreach(StatType type in System.Enum.GetValues(typeof(StatType))){
            if (a.GetAdd(type) > b.GetAdd(type)) c.SetAdd(type, a.GetAdd(type));
            else c.SetAdd(type, b.GetAdd(type));
            if (a.GetMult(type) > b.GetMult(type)) c.SetMult(type, a.GetMult(type));
            else c.SetMult(type, b.GetMult(type));
        }
        return c;
    }

    public static StatPack Min(StatPack a, StatPack b){
        StatPack c = new StatPack();
        foreach(StatType type in System.Enum.GetValues(typeof(StatType))){
            if (a.GetAdd(type) < b.GetAdd(type)) c.SetAdd(type, a.GetAdd(type));
            else c.SetAdd(type, b.GetAdd(type));
            if (a.GetMult(type) < b.GetMult(type)) c.SetMult(type, a.GetMult(type));
            else c.SetMult(type, b.GetMult(type));
        }
        return c;
    }

    public static StatPack Instead(StatPack basePack, StatPack currentPack, StatPack newPack){
        StatPack x = new StatPack();
        foreach(StatType type in System.Enum.GetValues(typeof(StatType))){
            x.SetAdd(type, (basePack.GetAdd(type)/(1+currentPack.GetMult(type)) - currentPack.GetAdd(type) + newPack.GetAdd(type)) * (1 + newPack.GetMult(type)));
        }
        return x;
    }

    public override string ToString()
    {
        string result = "Stat Pack\nADDS:\n";
        foreach(StatType type in System.Enum.GetValues(typeof(StatType))){
            result += System.Enum.GetName(typeof(StatType), type) + ": " + Adds[type] + "\n";
        }
        result += "MULTS:";
        foreach(StatType type in System.Enum.GetValues(typeof(StatType))){
            result += System.Enum.GetName(typeof(StatType), type) + ": " + Mults[type] + "\n";
        }

        return result;
    }

}