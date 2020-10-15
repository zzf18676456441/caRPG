using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIShoot : MonoBehaviour
{

    public float speed;
    public float stopDis;
    public float backDis;

    private float shotFrequencey;
    public float startShotTime;

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
        if(Vector2.Distance(transform.position, player.position) > stopDis)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }else if(Vector2.Distance(transform.position, player.position)<stopDis && Vector2.Distance(transform.position, player.position) > backDis)
        {
            transform.position = this.transform.position;
        }else if(Vector2.Distance(transform.position, player.position) < backDis)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, -speed * Time.deltaTime);
        }
        
        if(shotFrequencey <= 0)
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
