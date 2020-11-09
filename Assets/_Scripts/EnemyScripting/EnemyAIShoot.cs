using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIShoot : MonoBehaviour
{

    public float speed;
    public float stopDis = 15f;
    public float backDis = 10f;

    //if player out of this range, enemy will back to randomly roaming otherwise it will chase enemy and attack.
    public float chaseRange = 30f;
    public float stopChaseRange = 60f;

    private float shotFrequencey;
    public float startShotTime;
    public bool startChase = false;

    private bool isChasing = false;
    private bool isRetreating = false;

    public GameObject projectile;

    private Transform player;
    private SinglePointMovement spm;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("GameControllerObject").GetComponent<GameController>().GetCar().transform;
        spm = gameObject.GetComponent<SinglePointMovement>();
        spm.maxSpeed /= 5;
        shotFrequencey = startShotTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (startChase)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (isChasing && distance < stopDis)
            {
                isChasing = false;
                spm.Stop();
                gameObject.GetComponent<ShooterAnimator>().Idle();
            }
            if (!isRetreating && distance < backDis)
            {
                isRetreating = true;
                spm.MoveByVector(transform.position - player.position);
                gameObject.GetComponent<ShooterAnimator>().Walk();
            }
            if (isRetreating && distance > stopDis)
            {
                isRetreating = false;
                isChasing = true;
                spm.Chase(player);
                gameObject.GetComponent<ShooterAnimator>().Walk();
            }

            if (shotFrequencey <= 0)
            {
                GameObject shot = Instantiate(projectile, transform.position, Quaternion.identity);
                shot.GetComponent<Projectile>().FireAt(player.position - transform.position);
                shotFrequencey = startShotTime;
                gameObject.GetComponent<ShooterAnimator>().Shoot();
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
        if ((!startChase) && Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            startChase = true;
            gameObject.GetComponent<EnemyRoamAI>().enabled = false;
            spm.maxSpeed *= 5;
            isChasing = true;
            spm.Chase(player);
            spm.LookAt(player);
            return;
        }

        if (startChase && Vector2.Distance(transform.position, player.position) > stopChaseRange)
        {
            startChase = false;
            spm.maxSpeed /= 5;
            spm.StopLooking();
            gameObject.GetComponent<EnemyRoamAI>().enabled = true;
            return;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopChaseRange);
    }

}
