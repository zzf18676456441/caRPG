/*
*   FOR THE POWERUP
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMain : MonoBehaviour
{
    private bool attached;
    private PowerupManager appliedManager;

    private bool isOwned = false;
    private bool isChecked = false;
    private bool isEquipped = false;

    public bool IsOwned(){ return isOwned;}
    public bool IsChecked(){return isChecked;}
    public bool IsEquipped(){return isEquipped;}

    public void SetOwned(bool owned){
        isOwned = owned;
    }

    public void Check(){isChecked = true;}
    public void Equip(){isEquipped = true;}
    public void UnEquip(){isEquipped = false;}

    void Awake(){
        attached = false;
    }


    public void SetManager(PowerupManager _appliedManager){
        appliedManager = _appliedManager;
        attached = true;
    }

    public void ApplyTo(PowerupManager car)
    {
        if(attached) return;
        attached = true;

        PowerupAttachable pAttachable = GetComponent<PowerupAttachable>();
        PowerupEvent pEvent = GetComponent<PowerupEvent>();
        PowerupBoost pBoost = GetComponent<PowerupBoost>();
        PowerupStats pStats = GetComponent<PowerupStats>();

        if (pAttachable != null)
        {
            appliedManager = car.Attach(pAttachable, pAttachable.attachType);
        }
        else if (pEvent != null)
        {
            appliedManager = car.AddEvent(pEvent);
        }
        else if (pBoost != null)
        {
            appliedManager = car.ApplyBoost(pBoost);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        PowerupAttachable attachable = gameObject.GetComponent<PowerupAttachable>();
        if(attachable != null){
            appliedManager.NotifyCollision(collision, attachable);
        } else {
            if (collision.otherCollider.gameObject.tag == "Player"){
                ApplyTo(collision.otherCollider.gameObject.GetComponent<PowerupManager>());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        if (other.gameObject.tag == "Player" && !attached){
            ApplyTo(other.gameObject.GetComponent<PowerupManager>());
        }
        if (attached){
            PowerupMain otherPowerup = other.GetComponent<PowerupMain>();
            if (otherPowerup != null){
                otherPowerup.ApplyTo(appliedManager);
            }
        }
    }


}
