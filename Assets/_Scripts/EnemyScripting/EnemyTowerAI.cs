﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTowerAI : MonoBehaviour
{
    public float attackRange;
    public float startShotTime_Mode1;
    public float startShotTime_Mode2;
    public float startShotTime_Mode3;
    public GameObject projectile1;
    public GameObject projectile2;
    public GameObject projectile3;

    private Transform player;
    private float rotationCount = 0f;

    private float shotFrequencey_Mode1;
    private float shotFrequencey_Mode2;
    private float shotFrequencey_Mode3;

    void Start()
    {
        player = GameObject.Find("GameControllerObject").GetComponent<GameController>().GetCar().transform;
        shotFrequencey_Mode1 = startShotTime_Mode1;
        shotFrequencey_Mode2 = startShotTime_Mode2;
        shotFrequencey_Mode3 = startShotTime_Mode3;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if ( distance < attackRange)
        {
            shoot_one();
            shoot_three();
        }
        shoot_two();
    }

    private void shoot_one()
    {
        if (shotFrequencey_Mode1 <= 0)
        {
            GameObject shot = Instantiate(projectile1, transform.position, Quaternion.identity);
            shot.GetComponent<Projectile>().FireAt(player.position - transform.position);
            shotFrequencey_Mode1 = startShotTime_Mode1;
        }
        else
        {
            shotFrequencey_Mode1 -= Time.deltaTime;
        }
    }

    private void shoot_two()
    {
        if (shotFrequencey_Mode2 <= 0)
        {
            GameObject shot = Instantiate(projectile2, transform.position, Quaternion.identity);
            shot.GetComponent<Projectile>().FireAt(transform.up);
            GameObject shot2 = Instantiate(projectile2, transform.position, Quaternion.identity);
            shot2.GetComponent<Projectile>().FireAt(-transform.up);
            GameObject shot3 = Instantiate(projectile2, transform.position, Quaternion.identity);
            shot3.GetComponent<Projectile>().FireAt(transform.right);
            GameObject shot4 = Instantiate(projectile2, transform.position, Quaternion.identity);
            shot4.GetComponent<Projectile>().FireAt(-transform.right);
            shotFrequencey_Mode2 = startShotTime_Mode2;
            if(rotationCount >= 360f)
            {
                rotationCount = 0f;
            }else
            {
                rotationCount += 45f;
            }
            transform.rotation = Quaternion.Euler(0, 0, rotationCount);
        }
        else
        {
            shotFrequencey_Mode2 -= Time.deltaTime;
        }
    }

    private void shoot_three()
    {
        if (shotFrequencey_Mode3 <= 0)
        {
            GameObject shot = Instantiate(projectile3, transform.position, Quaternion.identity);
            shot.GetComponent<Projectile>().FireAt(player.transform);

            shotFrequencey_Mode3 = startShotTime_Mode3;
        }
        else
        {
            shotFrequencey_Mode3 -= Time.deltaTime;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
