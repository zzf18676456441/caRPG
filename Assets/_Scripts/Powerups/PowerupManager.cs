/*
*  FOR THE CAR
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Joint {FBumper, FLTire, FRTire, LDoor, RDoor, Center, BLTire, BRTire, RBumper, Bumpers, Engine, Frame, Tires, Trunk, Hood}

public enum AttachType {Fixed, Spring, NonPhysical}


public class PowerupManager : MonoBehaviour, IDamager
{
    private Dictionary<Joint, FixedJoint2D> joints;
    private Dictionary<Joint, PowerupAttachable> equipment;
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

        equipment = new Dictionary<Joint, PowerupAttachable>();

        equipment.Add(Joint.Bumpers, null);
        equipment.Add(Joint.Engine, null);
        equipment.Add(Joint.Frame, null);
        equipment.Add(Joint.Tires, null);
        equipment.Add(Joint.Trunk, null);

        statBoosts = new Dictionary<PowerupStats, bool>();
    }


    public PowerupManager Attach(Joint location, PowerupAttachable attachment, AttachType aType){
        
        if (attachment.isWeapon)
        {
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
        else
        {
            equipment[location] = attachment;
        }
        
        return this;
    }

    public PowerupManager Dettach(Joint location, PowerupAttachable attachment, AttachType aType)
    {

        if (attachment.isWeapon)
        {
            FixedJoint2D tJoint = joints[location];
            tJoint = gameObject.AddComponent<FixedJoint2D>();
            tJoint.anchor = new Vector2(gameObject.GetComponent<BoxCollider2D>().size.x / 2, -gameObject.GetComponent<Driving>().rearAxleDistance);
            tJoint.enabled = false;
        }
        else
        {
            equipment[location] = null;
        }

        return this;
    }

    public PowerupManager ApplyStats(PowerupStats stats)
    {
        //statBoosts[false].Add(stats);
        statBoosts.Add(stats, true);
        activateStatBoost(stats);
        if (stats.isPowerUp)
        {
            stats.gameObject.SetActive(false);
        }
        return this;
    }

    public PowerupManager RemoveStats(PowerupStats stats)
    {
        statBoosts[stats] = false;
        deactivateStatBoost(stats);
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
        gameObject.GetComponent<Rigidbody2D>().mass += stats.weightAdd;

        //Change Armor
        controller.GetPlayer().currentArmor += stats.maxArmorAdd;
    }

    private void deactivateStatBoost(PowerupStats stats)
    {
        //Change maxHealth
        controller.GetPlayer().maxHealth -= stats.maxHealthAdd;
        controller.GetPlayer().currentHealth -= stats.maxHealthAdd;

        //Change Acceleration
        gameObject.GetComponent<Driving>().acceleration -= stats.accelerationAdd;

        //Change MaxNO2
        controller.GetPlayer().maxNO2 -= stats.maxNO2Add;
        controller.GetPlayer().currentNO2 -= stats.maxNO2Add;

        //Change Weight
        gameObject.GetComponent<Rigidbody2D>().mass -= stats.weightAdd;

        //Change Armor
        controller.GetPlayer().currentArmor += stats.maxArmorAdd;
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
