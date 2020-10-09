using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePointMovement : MonoBehaviour
{
    public float maxSpeed = 20;
    public float acceleration = 2;
    public float rotationSpeed = 30;

    public void MoveByVector(Vector2 desiredDirection){
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 force = desiredDirection.normalized * (acceleration + rb.drag) * rb.mass;
        rb.AddForce(force);
    }
}
