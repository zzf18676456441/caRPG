using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    public float attackRange = 50f;
    public float startShotTime = 1f;
    public GameObject projectile;
    
    private Transform player;

    private float shotFrequencey;
    GameController controller;

    private bool tracking = false;

    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = controller.GetCar().transform;
        shotFrequencey = startShotTime;
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (tracking) {
            if(distance > attackRange) {
                tracking = false;
                GetComponent<SinglePointMovement>().StopLooking();
            }   
            else {
                shoot_one();
            }
        } else if(distance <= attackRange) {
            tracking = true;
            GetComponent<SinglePointMovement>().LookAt(player);
        }
        
    }


    private void shoot_one()
    {
        if (shotFrequencey <= 0)
        {
            GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
            shot.GetComponent<Projectile>().FireAt(transform.up);
            shotFrequencey = startShotTime;
        }
        else
        {
            shotFrequencey -= Time.deltaTime;
        }
    }
}
