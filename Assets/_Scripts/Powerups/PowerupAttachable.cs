using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupAttachable : MonoBehaviour, IDamager
{
    [Header("Attachment Location")]
    public bool isWeapon;
    public WeaponMount weaponLocation = WeaponMount.Grill;
    public ModMount modLocation = ModMount.Bumpers;
    public AttachType attachType = AttachType.Fixed;

    
    public float baseDamage = 50f;
    public DamageType damageType = DamageType.VelocityAmplified;
    public DamageFlag[] damageFlags = { DamageFlag.Impact };

    


    [Tooltip("Leave 0,0 to auto-calculate")]
    public Vector2 anchorOverride;
    
    // Auto-calc anchor location
    public void SetAnchor(Joint attachLocation)
    {
        if (anchorOverride.x != 0 || anchorOverride.y != 0) return;
        switch(attachLocation){
            case Joint.BLTire:
            case Joint.FLTire:
            case Joint.LDoor:
                anchorOverride = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x/2,0f);
                break;
            
            case Joint.BRTire:
            case Joint.FRTire:
            case Joint.RDoor:
                anchorOverride = new Vector2(-gameObject.GetComponent<BoxCollider2D>().size.x/2,0f);
                break;

            case Joint.FBumper:
                anchorOverride = new Vector2(0f,-gameObject.GetComponent<BoxCollider2D>().size.y/2);
                break;
            
            case Joint.RBumper:
                anchorOverride = new Vector2(0f,gameObject.GetComponent<BoxCollider2D>().size.y/2);
                break;

            case Joint.Center:
            case Joint.Hood:
            case Joint.Trunk:
            default:
                anchorOverride = new Vector2(0f, 0f);
            break;
        }
        
    }

    public Damage GetDamage(){
        Damage result = new Damage(baseDamage, damageType, this.gameObject);
        foreach (DamageFlag flag in damageFlags)
        {
            result.AddDamageFlag(flag);
        }
        return result;
    }
}


