using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupAttachable : MonoBehaviour
{
    [Header("Attachment Location")]
    public Joint attachLocation = Joint.Center;
    public AttachType attachType = AttachType.Fixed;
    public bool isWeapon;

    [Tooltip("Leave 0,0 to auto-calculate")]
    public Vector2 anchorOverride;
    
    // Auto-calc anchor location
    void Awake()
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
}
