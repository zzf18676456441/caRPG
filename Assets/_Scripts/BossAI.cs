using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    public EnemyAIShoot firingAI;
    public EnemyAI chargingAI;
    public float firingTimer;
    public Animator anim;
    private bool animChanged;
    private float timeFired;
    private Transform player;

    void Awake()
    {
        player = GameObject.Find("GameControllerObject").GetComponent<GameController>().GetCar().transform;
        timeFired = firingTimer;
    }
    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > 70)
        {
            bool swapped = chargingAI;
            chargingAI.enabled = false;
            firingAI.enabled = true;
            swapped = chargingAI && swapped;
            animChanged = swapped;
        }
        else
        {
            bool swapped = chargingAI;
            chargingAI.enabled = true;
            firingAI.enabled = false;
            chargingAI.startChase = true;
            timeFired = firingTimer;
            swapped = chargingAI && swapped;
            animChanged = swapped;
        }
        if (firingAI.enabled && timeFired < 0)
        {
            bool swapped = chargingAI;
            chargingAI.enabled = true;
            firingAI.enabled = false;
            chargingAI.startChase = true;
            swapped = chargingAI && swapped;
            animChanged = swapped;
        }
        else if (firingAI.enabled && timeFired >= 0)
        {
            timeFired -= Time.deltaTime;
        }
        if (chargingAI.enabled && animChanged)
        {
            anim.SetBool("isCharging", true);
            anim.Play("BossWalk");
            animChanged = false;
        }
        else if (animChanged)
        {
            anim.SetBool("isCharging", false);
            anim.Play("BossFiring");
            animChanged = false;
        }
    }
}
