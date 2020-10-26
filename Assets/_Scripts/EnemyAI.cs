using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public float speed = 0;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool wait = false;
    public bool startChase = false;
    //if player out of this range, enemy will back to randomly roaming otherwise it will chase enemy and attack.
    public float chaseRange = 30f;

    private Transform player;


    GameController controller;
    void Awake(){
        rb = gameObject.GetComponent<Rigidbody2D>();
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }
    // Start is called before the first frame update
    void Start()
    {
        player = controller.GetCar().transform;

    }

    // Update is called once per frame
    void Update()
    {
        if (!wait)
        {
            if (startChase)
            {
                Vector3 dir = player.position - transform.position;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                rb.rotation = angle;
                dir.Normalize();
                movement = new Vector2(dir.x, dir.y);
                chase(movement);
            }
        }
    }

    private void FixedUpdate()
    {
        FindTarget();
    }

    private void FindTarget()
    {
        if (Vector2.Distance(transform.position, player.position) < chaseRange)
        {
            startChase = true;
            gameObject.GetComponent<EnemyRoamAI>().enabled = false;
        }
        else if(startChase)
        {
            startChase = false;
            gameObject.GetComponent<EnemyRoamAI>().enabled = true;
        }
    }

    void chase(Vector2 dir)
    {
        GetComponent<SinglePointMovement>().MoveByVector(dir);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(Wait());
        }
    }

    IEnumerator Wait()
    {
        wait = true;
        yield return new WaitForSeconds(1);
        wait = false;
    }

}
