using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Joint {FBumper, FLTire, FRTire, Hood, 
    LDoor, RDoor, Center, BLTire, BRTire, Trunk, RBumper}


public class Attachments : MonoBehaviour
{
    private Dictionary<Joint, FixedJoint2D> joints;

    void Awake(){
        joints = new Dictionary<Joint, FixedJoint2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        FixedJoint2D tJoint;
        float lLength, lWidth, lFrontAxle, lRearAxle;
        lLength = gameObject.GetComponent<BoxCollider2D>().size.y;
        lWidth = gameObject.GetComponent<BoxCollider2D>().size.x;
        lFrontAxle = gameObject.GetComponent<Driving>().frontAxleDistance;
        lRearAxle = gameObject.GetComponent<Driving>().rearAxleDistance;
        

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.enabled = false;
        joints.Add(Joint.Center, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(0f,lLength/2);
        tJoint.enabled = false;
        joints.Add(Joint.FBumper, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(0f,-lLength/2);
        tJoint.enabled = false;
        joints.Add(Joint.RBumper, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(-lWidth/2,lFrontAxle);
        tJoint.enabled = false;
        joints.Add(Joint.FLTire, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(lWidth/2,lFrontAxle);
        tJoint.enabled = false;
        joints.Add(Joint.FRTire, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(-lWidth/2,-lRearAxle);
        tJoint.enabled = false;
        joints.Add(Joint.BLTire, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(lWidth/2,-lRearAxle);
        tJoint.enabled = false;
        joints.Add(Joint.BRTire, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(-lWidth/2,lFrontAxle-lRearAxle);
        tJoint.enabled = false;
        joints.Add(Joint.LDoor, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(lWidth/2,lFrontAxle-lRearAxle);
        tJoint.enabled = false;
        joints.Add(Joint.RDoor, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(0f,lFrontAxle);
        tJoint.enabled = false;
        joints.Add(Joint.Hood, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(0f,-lRearAxle);
        tJoint.enabled = false;
        joints.Add(Joint.Trunk, tJoint);


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
