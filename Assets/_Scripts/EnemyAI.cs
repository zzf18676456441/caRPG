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
        //GetComponent<SinglePointMovement>().Chase(player);
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    private void FixedUpdate()
    {

    }

    void chase(Vector2 dir)
    {
     
    }
}
