using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIShoot : MonoBehaviour
{

    public float speed;
    public float stopDis;
    public float backDis;

    //if player out of this range, enemy will back to randomly roaming otherwise it will chase enemy and attack.
    public float chaseRange = 30f;

    private float shotFrequencey;
    public float startShotTime;
    public bool startChase = false;

    public GameObject projectile;

    private Transform player;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("GameControllerObject").GetComponent<GameController>().GetCar().transform;
        shotFrequencey = startShotTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (startChase)
        {
            if (Vector2.Distance(transform.position, player.position) > stopDis)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
            }
            else if (Vector2.Distance(transform.position, player.position) < stopDis && Vector2.Distance(transform.position, player.position) > backDis)
            {
                transform.position = this.transform.position;
            }
            else if (Vector2.Distance(transform.position, player.position) < backDis)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
            }

            if (shotFrequencey <= 0)
            {
                Instantiate(projectile, transform.position, Quaternion.identity);
                shotFrequencey = startShotTime;
            }
            else
            {
                shotFrequencey -= Time.deltaTime;
            }
        }
        
    }

    void FixedUpdate()
    {
        FindTarget();
    }

    private void FindTarget()
    {
        if (Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            Debug.Log("find it!");
            startChase = true;
            gameObject.GetComponent<EnemyRoamAI>().enabled = false;
        }
        else if (startChase)
        {
            startChase = false;
            gameObject.GetComponent<EnemyRoamAI>().enabled = true;
        }
    }
}
