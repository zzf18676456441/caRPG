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
    //private Dictionary<bool, List<PowerupStats>> statBoosts;
    private Dictionary<PowerupStats, bool> statBoosts;
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

        /*tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(0f,lFrontAxle);
        tJoint.enabled = false;
        joints.Add(Joint.Hood, tJoint);

        tJoint = gameObject.AddComponent<FixedJoint2D>();
        tJoint.anchor = new Vector2(0f,-lRearAxle);
        tJoint.enabled = false;
        joints.Add(Joint.Trunk, tJoint);*/

        //statBoosts = new Dictionary<bool, List<PowerupStats>>();
        //statBoosts.Add(true, new List<PowerupStats>());
        //statBoosts.Add(false, new List<PowerupStats>());

        statBoosts = new Dictionary<PowerupStats, bool>();
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
        //statBoosts[false].Add(stats);
        statBoosts.Add(stats, true);
        activateStatBoost(stats);
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
        /*if (statBoosts[false].Count > 0)
        {
            for (int i = 0; i < statBoosts[false].Count; i++)
            {
                activateStatBoost(statBoosts[false][i]);
                statBoosts[true].Add(statBoosts[false][i]);
            }
            statBoosts[false].Clear();
        }*/
    }

    private void activateStatBoost(PowerupStats stats)
    {
        //Change maxHealth
        controller.GetPlayer().maxHealth += stats.maxHealthAdd;
        controller.GetPlayer().currentHealth += stats.maxHealthAdd;

        //Change Health
        if (controller.GetPlayer().currentHealth + stats.healthAdd >= controller.GetPlayer().maxHealth)
        {
            controller.GetPlayer().currentHealth = controller.GetPlayer().maxHealth;
        }
        else
        {
            controller.GetPlayer().currentHealth += stats.healthAdd;
        }

        //Change Acceleration
        gameObject.GetComponent<Driving>().acceleration += stats.accelerationAdd;

        //Change MaxNO2
        controller.GetPlayer().maxNO2 += stats.maxNO2Add;
        controller.GetPlayer().currentNO2 += stats.maxNO2Add;

        //Change Weight


        //Change Grip

    }

    public void NotifyCollision(Collision2D collision){
        // Can the thing I hit deal damage?
        IDamager damager = (IDamager)collision.collider.GetComponent(typeof(IDamager));

        // Can the thing I hit take damage?
        IDamagable damagable = (IDamagable)collision.collider.GetComponent(typeof(IDamagable));

        // If it can deal damage, apply that damage to the player.
        if (damager != null) {
            DamageSystem.ApplyDamage(damager, controller.GetPlayer(), collision.relativeVelocity);
        }        

        // If it can take damage, apply this object's damage to it.
        if (damagable != null) {
            DamageSystem.ApplyDamage(this, damagable, collision.relativeVelocity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        NotifyCollision(collision);
    }

    
    private void OnTriggerEnter2D(Collider2D other){
        IDamager damager = (IDamager)other.GetComponent(typeof(IDamager));
        if (damager != null) {
            DamageSystem.ApplyDamage(damager, controller.GetPlayer());
        }        
    }

    public Damage GetDamage(){
        Damage damage = new Damage(50f, DamageType.VelocityAmplified, this.gameObject);
        damage.AddDamageFlag(DamageFlag.Impact);
        return damage;
    }
}
