using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericDamager : MonoBehaviour, IDamager
{
    // NOTE:  FOR MORE COMPLEX INTERACTIONS, JUST HAVE THE COMPONENT
    // IMPLEMENT THE IDAMAGER INTERFACE
    public float baseDamage;
    public DamageType type;
    public DamageFlag[] flags;

    public float knockbackForce = 0f;
   

    
    public Damage GetDamage(){
        Damage result = new Damage(baseDamage, type, this.gameObject);
        foreach (DamageFlag flag in flags){
            result.AddDamageFlag(flag);
        }
        result.knockbackForce = knockbackForce;
        return result;
    }
}
