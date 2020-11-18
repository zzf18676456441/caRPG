/*
*  FOR THE CAR
*/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Joint {FBumper, FLTire, FRTire, LDoor, RDoor, Center, BLTire, BRTire, RBumper, Bumpers, Engine, Frame, Tires, Trunk, Hood}
public enum AttachType {Fixed, Spring, NonPhysical}
public enum WeaponMount {Wheels, Doors, Grill, Hitch, Roof}
public enum ModMount {Tires, Engine, Bumpers, Frame, Trunk}


public class PowerupManager : MonoBehaviour, IDamager
{
    private Dictionary<Joint, FixedJoint2D> joints;
    private Dictionary<WeaponMount, PowerupAttachable> equippedWeapons;
    private Dictionary<ModMount, PowerupAttachable> equippedMods;
    private Dictionary<WeaponMount, List<GameObject>> physicalWeapons;
    
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

        equippedWeapons = new Dictionary<WeaponMount, PowerupAttachable>();
        equippedMods = new Dictionary<ModMount, PowerupAttachable>();
        physicalWeapons = new Dictionary<WeaponMount, List<GameObject>>();
    }


    public PowerupManager Attach(PowerupAttachable attachment, AttachType aType){
        if (attachment.isWeapon){
            WeaponMount slot = attachment.weaponLocation;
            List<Joint> attachJoints = new List<Joint>();
            List<GameObject> newWeapons = new List<GameObject>();
            equippedWeapons[slot] = attachment;
            if (physicalWeapons.ContainsKey(slot)){
                foreach(GameObject oldWeapon in physicalWeapons[slot]){
                    Destroy(oldWeapon);
                }
            }
            
            switch(slot){
                case WeaponMount.Grill:
                    attachJoints.Add(Joint.FBumper);
                break;
                case WeaponMount.Hitch:
                    attachJoints.Add(Joint.RBumper);
                break;
                case WeaponMount.Doors:
                    attachJoints.Add(Joint.LDoor);
                    attachJoints.Add(Joint.RDoor);
                break;
                case WeaponMount.Roof:
                    attachJoints.Add(Joint.Center);
                break;
                case WeaponMount.Wheels:
                    attachJoints.Add(Joint.FLTire);
                    attachJoints.Add(Joint.FRTire);
                    attachJoints.Add(Joint.BLTire);
                    attachJoints.Add(Joint.BRTire);
                break;
            }

            foreach(Joint joint in attachJoints){
                FixedJoint2D tJoint = joints[joint];
                PowerupAttachable newAttachment = Instantiate(attachment.gameObject).GetComponent<PowerupAttachable>();
                newAttachment.GetComponent<PowerupMain>().SetManager(this);
                newWeapons.Add(newAttachment.gameObject);
                newAttachment.SetAnchor(joint);
                newAttachment.gameObject.transform.position = gameObject.transform.position + new Vector3(tJoint.anchor.x, tJoint.anchor.y, 0f);
                
                switch (aType)
                {
                    case AttachType.Spring:
                        tJoint.connectedBody = newAttachment.gameObject.GetComponent<Rigidbody2D>();
                        tJoint.connectedAnchor = newAttachment.anchorOverride;
                        tJoint.enabled = true;
                    break;

                    case AttachType.Fixed:
                    default:
                        newAttachment.gameObject.transform.parent = gameObject.transform;
                    break;
                }
            }

            physicalWeapons[slot] = newWeapons;

        } else {
            equippedMods[attachment.modLocation] = attachment;
        }
        
        ReCalcStats();

        return this;
    }

    public void ReCalcStats(){
        PowerupStats powerupStats = gameObject.AddComponent<PowerupStats>();
        foreach(PowerupAttachable attachment in equippedWeapons.Values){
            PowerupStats newStats = attachment.GetComponent<PowerupStats>();
            if(newStats != null){
                powerupStats.maxHealthAdd += newStats.maxHealthAdd;
                powerupStats.maxHealthMult += newStats.maxHealthMult;
                powerupStats.maxArmorAdd += newStats.maxArmorAdd;
                powerupStats.maxArmorMult += newStats.maxArmorMult;
                powerupStats.maxNO2Add += newStats.maxNO2Add;
                powerupStats.maxNO2Mult += newStats.maxNO2Mult;
                powerupStats.weightAdd += newStats.weightAdd;
                powerupStats.gripAdd += newStats.gripAdd;
                powerupStats.gripMult += newStats.gripMult;
                powerupStats.accelerationAdd += newStats.accelerationAdd;
                powerupStats.accelerationMult += newStats.accelerationMult;
                powerupStats.topSpeedAdd += newStats.topSpeedAdd;
                powerupStats.topSpeedMult += newStats.topSpeedMult;
            }
        }
        controller.GetPlayer().ReApplyStats(powerupStats);
        Destroy(powerupStats);
    }


    public PowerupManager ApplyBoost(PowerupBoost boost)
    {
        controller.GetPlayer().currentHealth += boost.healthBoost;
        controller.GetPlayer().currentNO2 += boost.nO2Boost;
        return this;
    }

    public PowerupManager AddEvent(PowerupEvent pEvent){
        //TODO:  Do something
        return this;
    }

    public void NotifyCollision(Collision2D collision, IDamager hitter){
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
            DamageSystem.ApplyDamage(hitter, damagable, collision.relativeVelocity);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        NotifyCollision(collision, this);
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
