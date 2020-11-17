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

    void Awake(){
        attached = false;
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
            appliedManager = car.Attach(pAttachable.attachLocation, pAttachable, pAttachable.attachType);
            if (pStats != null)
            {
                appliedManager = appliedManager.ApplyStats(pStats);
            }
        }
        else if (pEvent != null)
        {
            appliedManager = car.AddEvent(pEvent);
        }
        else if (pBoost != null)
        {
            appliedManager = car.ApplyBoost(pBoost);
        }
        else if (pStats != null)
        {
            appliedManager = car.ApplyStats(pStats);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(attached){
            appliedManager.NotifyCollision(collision);
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
