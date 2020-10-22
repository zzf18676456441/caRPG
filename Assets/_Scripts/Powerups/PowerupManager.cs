/*
*  FOR THE CAR
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Joint {FBumper, FLTire, FRTire, Hood, 
    LDoor, RDoor, Center, BLTire, BRTire, Trunk, RBumper}

public enum AttachType {Fixed, Spring, NonPhysical}


public class PowerupManager : MonoBehaviour, IDamager
{
    private Dictionary<Joint, FixedJoint2D> joints;
    private Dictionary<bool, List<PowerupStats>> statBoosts;
    GameController controller;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
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


    public PowerupManager Attach(Joint location, PowerupAttachable attachment, AttachType aType){
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
        return this;
    }

    public PowerupManager ApplyStats(PowerupStats stats)
    {
        //gameObject.GetComponent<Driving>().acceleration = 20;
        statBoosts[false].Add(stats);
        stats.gameObject.SetActive(false);
        return this;
    }

    public PowerupManager ApplyBoost(PowerupBoost boost)
    {
        //TODO:  Do something
        return this;
    }

    public PowerupManager AddEvent(PowerupEvent pEvent){
        //TODO:  Do something
        return this;
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
        controller.GetPlayer().currentHealth += stats.healthAdd;
        gameObject.GetComponent<Driving>().acceleration += stats.accelerationAdd;
    }

    public void NotifyCollision(Collision2D collision){
        // Can the thing I hit deal damage?
        IDamager damager = (IDamager)collision.collider.GetComponent(typeof(IDamager));

        // Can the thing I hit take damage?
        IDamagable damagable = (IDamagable)collision.collider.GetComponent(typeof(IDamagable));

        // If it can deal damage, apply that damage to the player.
        if (damager != null) {
            controller.HandleDamage(damager, controller.GetPlayer(), collision.relativeVelocity.magnitude);
        }        

        // If it can take damage, apply this object's damage to it.
        if (damagable != null) {
            controller.HandleDamage(this, damagable, collision.relativeVelocity.magnitude);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        NotifyCollision(collision);
    }

    
    private void OnTriggerEnter2D(Collider2D other){
        IDamager damager = (IDamager)other.GetComponent(typeof(IDamager));
        if (damager != null) {
            controller.HandleDamage(damager, controller.GetPlayer(), 0f);
        }        
    }

    public Damage GetDamage(){
        Damage damage = new Damage(50f, DamageType.Velocity);
        damage.AddDamageFlag(DamageFlag.Piercing);
        return damage;
    }
}
