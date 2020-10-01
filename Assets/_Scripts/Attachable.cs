using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attachable : MonoBehaviour
{
    [Header("Attachment Location")]
    public Joint attachLocation = Joint.Center;
    public Vector2 anchor;
    
    // Start is called before the first frame update
    void Awake()
    {
        switch(attachLocation){
            case Joint.BLTire:
            case Joint.FLTire:
            case Joint.LDoor:
                anchor = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x/2,0f);
                break;
            
            case Joint.BRTire:
            case Joint.FRTire:
            case Joint.RDoor:
                anchor = new Vector2(-gameObject.GetComponent<BoxCollider2D>().size.x/2,0f);
                break;

            case Joint.FBumper:
                anchor = new Vector2(0f,-gameObject.GetComponent<BoxCollider2D>().size.y/2);
                break;
            
            case Joint.RBumper:
                anchor = new Vector2(0f,gameObject.GetComponent<BoxCollider2D>().size.y/2);
                break;

            case Joint.Center:
            case Joint.Hood:
            case Joint.Trunk:
            default:
                anchor = new Vector2(0f, 0f);
            break;

        }
        
    }

    void Start(){
        GameObject.Find("car").GetComponent<Attachments>().Attach(attachLocation,this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
