/*
*   FOR THE POWERUP
*/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupMain : MonoBehaviour
{
    public void ApplyTo(PowerupManager car)
    {
        PowerupAttachable pAttachable = GetComponent<PowerupAttachable>();
        PowerupEvent pEvent = GetComponent<PowerupEvent>();
        PowerupBoost pBoost = GetComponent<PowerupBoost>();
        PowerupStats pStats = GetComponent<PowerupStats>();

        if (pAttachable != null)
        {
            car.Attach(pAttachable.attachLocation, pAttachable, pAttachable.attachType);
        }

        if (pEvent != null)
        {
            //TODO:  Event-driven powerups, i.e. create an explosion upon collision
        }

        if (pBoost != null)
        {
            //TODO:  Boost-type powerups, i.e. healing effect or temporary speed
        }

        if (pStats != null)
        {
            car.Attach(pStats);
        }
    }
}
