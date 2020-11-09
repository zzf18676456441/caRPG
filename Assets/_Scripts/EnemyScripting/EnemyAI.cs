using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private bool wait = false;
    public bool startChase = false;
    //if player out of this range, enemy will back to randomly roaming otherwise it will chase enemy and attack.
    public float chaseRange = 30f;
    public float stopChaseRange = 50f;

    private Transform player;


    GameController controller;
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = controller.GetCar().transform;
        gameObject.GetComponent<SinglePointMovement>().maxSpeed /= 5;
    }

    // Update is called once per frame
    void Update()
    {
        if (!wait)
        {
            if (startChase)
            {
                gameObject.GetComponent<SinglePointMovement>().Chase(player);
            }
        }
    }

    private void FixedUpdate()
    {
        FindTarget();
    }

    private void FindTarget()
    {
        if ((!startChase) && Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            startChase = true;
            gameObject.GetComponent<EnemyRoamAI>().enabled = false;
            gameObject.GetComponent<SinglePointMovement>().maxSpeed *= 5;
            return;
        }

        if (startChase && Vector2.Distance(transform.position, player.position) > stopChaseRange)
        {
            startChase = false;
            gameObject.GetComponent<SinglePointMovement>().maxSpeed /= 5;
            gameObject.GetComponent<EnemyRoamAI>().enabled = true;
            return;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<SinglePointMovement>().Stop();
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        wait = true;
        yield return new WaitForSeconds(1);
        wait = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, chaseRange);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, stopChaseRange);
    }

}
