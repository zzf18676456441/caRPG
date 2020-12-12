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

    public float baseDamage = 20f;    
    GameController controller;


    public PowerupAttachable GetMod(ModMount mount){ 
        if (equippedMods.ContainsKey(mount))
            return equippedMods[mount];
        return null;
    }
    public PowerupAttachable GetWeapon(WeaponMount mount){
        if (equippedWeapons.ContainsKey(mount))
            return equippedWeapons[mount];
        return null;
    }

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

            if(equippedWeapons.ContainsKey(slot)){
                equippedWeapons[slot].GetComponent<PowerupMain>().UnEquip();
            }
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
                if (joint == Joint.BLTire || joint == Joint.FLTire || joint == Joint.LDoor){
                    newAttachment.gameObject.transform.localScale = new Vector3(-1,1,1);
                    newAttachment.gameObject.transform.position = gameObject.transform.position + new Vector3(tJoint.anchor.x, tJoint.anchor.y, 0f) - new Vector3(newAttachment.anchorOverride.x, newAttachment.anchorOverride.y,0);
                } else {
                    newAttachment.gameObject.transform.position = gameObject.transform.position + new Vector3(tJoint.anchor.x, tJoint.anchor.y, 0f) + new Vector3(newAttachment.anchorOverride.x, newAttachment.anchorOverride.y,0);
                }

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
            if(equippedMods.ContainsKey(attachment.modLocation)){
                equippedMods[attachment.modLocation].GetComponent<PowerupMain>().UnEquip();
            }
            equippedMods[attachment.modLocation] = attachment;
        }
        
        attachment.GetComponent<PowerupMain>().Equip();
        ReCalcStats();


        return this;
    }

    public void Remove(WeaponMount slot){
        Debug.Log("CALLED");
        if(equippedWeapons.ContainsKey(slot)){
            equippedWeapons[slot].GetComponent<PowerupMain>().UnEquip();
            equippedWeapons.Remove(slot);
        }
        if (physicalWeapons.ContainsKey(slot)){
            foreach(GameObject oldWeapon in physicalWeapons[slot]){
                Destroy(oldWeapon);
            }
        }
        ReCalcStats();
    }

    public void Remove(ModMount slot){
        if(equippedMods.ContainsKey(slot)){
            equippedMods[slot].GetComponent<PowerupMain>().UnEquip();
            equippedMods.Remove(slot);
        }
        ReCalcStats();
    }

    public float GetWeaponDamage(WeaponMount slot){
        if(equippedWeapons.ContainsKey(slot)) return equippedWeapons[slot].baseDamage;
        return baseDamage;
    }

    public void ReCalcStats(){
        StatPack powerupStats = new StatPack();
        foreach(PowerupAttachable attachment in equippedWeapons.Values){
            PowerupStats newStats = attachment.GetComponent<PowerupStats>();
            if(newStats != null){
                powerupStats += newStats.GetPack();
            }
        }
        foreach(PowerupAttachable attachment in equippedMods.Values){
            PowerupStats newStats = attachment.GetComponent<PowerupStats>();
            if(newStats != null){
                powerupStats += newStats.GetPack();
            }
        }
        controller.GetPlayer().ReApplyStats(powerupStats);
    }


    public PowerupManager ApplyBoost(PowerupBoost boost)
    {
        controller.GetPlayer().currentHealth += boost.healthBoost;
        controller.GetPlayer().currentNO2 += boost.nO2Boost;
        if(controller.GetPlayer().currentNO2 > controller.GetPlayer().maxNO2) 
            controller.GetPlayer().currentNO2 = controller.GetPlayer().maxNO2;
        Destroy(boost.gameObject);
        return this;
    }

    public PowerupManager AddEvent(PowerupEvent pEvent){
        //TODO:  Do something
        return this;
    }

    public void NotifyCollision(Collision2D collision, IDamager hitter){
        Debug.Log(collision.collider.gameObject.name);
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
            DamageSystem.ApplyDamage(damager, controller.GetPlayer(), transform.position - other.transform.position);
        }        
    }

    public Damage GetDamage(){
        Damage damage = new Damage(baseDamage, DamageType.VelocityAmplified, this.gameObject);
        damage.AddDamageFlag(DamageFlag.Impact);
        return damage;
    }
}
