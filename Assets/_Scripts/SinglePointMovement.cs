using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePointMovement : MonoBehaviour
{
    public float maxSpeed = 20;
    public float acceleration = 2;
    public float rotationSpeed = 30;
    private Rigidbody2D rb;


    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Moves along the desired vector.
    /// Continues moving until issued another command.
    /// </summary>
    /// <param name="desiredDirection">Direction to move in.</param>
    public void MoveByVector(Vector2 desiredDirection){
        Vector2 force = desiredDirection.normalized * (acceleration + rb.drag) * rb.mass;
        rb.AddForce(force);
    }

    /// <summary>
    /// Chases the target transform.
    /// Continues moving until issued another command.
    /// </summary>
    /// <param name="target">Transform to chase</param>
    public void Chase(Transform target){

    }

    /// <summary>
    /// Moves to a specific location, then stops.
    /// Continues moving until issued another command, or until destination is reached.
    /// </summary>
    /// <param name="location">Location to move to</param>
    public void MoveToLocation(Vector2 location){

    }

    /// <summary>
    /// Turns to face a specified transform.
    /// Will continue to look at the transform, even if it moves, until issued another command.
    /// Overrides normal looking behavior until stopped.
    /// </summary>
    /// <param name="target">Transform to look at</param>
    public void LookAt(Transform target){

    }

    /// <summary>
    /// Turns to face a specified location.
    /// Will continue to look at the location, until issued another command.
    /// Overrides normal looking behavior until stopped.
    /// </summary>
    /// <param name="location">Location to look at</param>
    public void LookAt(Vector2 location){

    }

    /// <summary>
    /// Rotates the object by the specified degrees.
    /// Overrides any other looking behavior.
    /// Will continue to rotate until direction is achieved, then will resume default behavior.
    /// </summary>
    /// <param name="degrees"></param>
    public void TurnBy(float degrees){

    }

    /// <summary>
    /// Object will come to a stop as fast as possible, then resume default behavior.
    /// </summary>
    public void Stop(){

    }

    /// <summary>
    /// Releases any current looking commands, then resumes normal behavior.
    /// </summary>
    public void StopLooking(){

    }

    /// <summary>
    /// NON-PHYSICAL MOVEMENT.
    /// Object will be completely stopped and move to the specified location.
    /// </summary>
    /// <param name="location">Location to teleport to</param>
    public void TeleportTo(Vector2 location){

    }

    /// <summary>
    /// NON-PHYSICAL MOVEMENT.
    /// Object will be completely stopped and move to the specified location.
    /// Object will be facing "facing" after movement.
    /// </summary>
    /// <param name="location">Location to teleport to</param>
    /// <param name="facing">Direction to face</param>
    public void TeleportTo(Vector2 location, Vector2 facing){

    }


    /// <summary>
    /// A complete movement instruction.
    /// Generated on command, followed until completion/stopped.
    /// </summary>
    private class Instruction{



        /// <summary>
        /// Makes the next movement.
        /// </summary>
        public void Move(){

        }
    }
}
