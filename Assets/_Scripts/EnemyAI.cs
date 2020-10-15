using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{

    public float speed = 0;
    private Rigidbody2D rb;
    private Vector2 movement;
    public bool wait = false;

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
    
    }

    private void FixedUpdate()
    {
            Vector3 dir = player.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            rb.rotation = angle;
            dir.Normalize();
            movement = dir;
            chase(movement);
    }

    void chase(Vector2 dir)
    {
        GetComponent<SinglePointMovement>().MoveByVector(dir);
    }
}
