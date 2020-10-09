﻿/*
*  FOR THE CAR
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Joint {FBumper, FLTire, FRTire, Hood, 
    LDoor, RDoor, Center, BLTire, BRTire, Trunk, RBumper}

public enum AttachType {Fixed, Spring}


public class PowerupManager : MonoBehaviour
{
    private Dictionary<Joint, FixedJoint2D> joints;
    private Dictionary<bool, List<PowerupStats>> statBoosts;

    // Start is called before the first frame update
    void Awake()
    {
        joints = new Dictionary<Joint, FixedJoint2D>();
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
        tJoint.enableCollision = true;
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

        statBoosts = new Dictionary<bool, List<PowerupStats>>();
        statBoosts.Add(true, new List<PowerupStats>());
        statBoosts.Add(false, new List<PowerupStats>());
    }


    public void Attach(Joint location, PowerupAttachable attachment, AttachType aType){
        FixedJoint2D tJoint = joints[location];
        attachment.gameObject.transform.position = gameObject.transform.position + new Vector3(tJoint.anchor.x, tJoint.anchor.y, 0f);

        switch (aType)
        {
            case AttachType.Spring:
            tJoint.connectedBody = attachment.gameObject.GetComponent<Rigidbody2D>();
            tJoint.connectedAnchor = attachment.anchorOverride;
            tJoint.enabled = true;
            break;

            case AttachType.Fixed:
            default:
            attachment.gameObject.transform.parent = gameObject.transform;
            break;
        }
    }

    public void Attach(PowerupStats stats)
    {
        //gameObject.GetComponent<Driving>().acceleration = 20;
        statBoosts[false].Add(stats);
        
    }

    void Update()
    {
        if (statBoosts[false].Count > 0)
        {
            for (int i = 0; i < statBoosts[false].Count; i++)
            {
                activateStatBoost(statBoosts[false][i]);
                statBoosts[true].Add(statBoosts[false][i]);
            }
            statBoosts[false].Clear();
        }
    }

    private void activateStatBoost(PowerupStats stats)
    {
        gameObject.GetComponent<Player>().health += stats.healthAdd;
        gameObject.GetComponent<Driving>().acceleration += stats.accelerationAdd;
    }
}
