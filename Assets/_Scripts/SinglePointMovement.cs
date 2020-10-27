using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePointMovement : MonoBehaviour
{
    public enum Direction{Up, Down, Left, Right}


    [Header ("Movement Settings")]
    [Tooltip ("Top speed, in meters per second (26 m/s = 60 mph)")]
    public float maxSpeed = 10;
    [Tooltip ("Acceleration, in meters per second^2 (5 m/s is reasonable for a car or a human)")]
    public float acceleration = 5;
    [Tooltip ("Rotation, in degrees per second (720 default)")]
    public float rotationSpeed = 720;
    [Header ("Sprite Settings")]
    [Tooltip ("Direction of sprite considered \"forward\"")]
    public Direction direction = Direction.Up;
    
    private Rigidbody2D rb;
    private int counter;
    private Instruction instruction;

    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// TAKES OVER RIGIDBODY DRAG PHYSICS
    /// </summary>
    void Start(){
        rb.drag = 0;
        rb.angularDrag = 0;
        instruction = new Instruction(this);
    }


    void FixedUpdate(){
        instruction.Follow();

//        counter++;
//        if(counter <= 30) rb.AddTorque(rb.inertia * rotationSpeed * Mathf.Deg2Rad);
//        if (counter == 220) RotationSetToMax(false);
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
        instruction.SetLookTarget(new Target(target));
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



    /*
    *
    *     HELPER FUNCTIONS  
    *
    */

    /// <summary>
    /// The angle the gameobject is currently facing.
    /// 0 degrees is up.
    /// Rotation is counter-clockwise (left).
    /// </summary>
    /// <returns>The angle, in degrees, clamped to [0-360)</returns>
    public float GetAngle(){
        Vector2 original = new Vector2(transform.up.x, transform.up.y);
        float result = Vector2.SignedAngle(new Vector2(0,1),original);
        switch (direction){
            case Direction.Right:
                result += 270;
            break;
            case Direction.Left:
                result += 90;
            break;
            case Direction.Down:
                result += 180;
            break;
            case Direction.Up:
            default:
            break;
        }
        if (result > 180) result -= 360;
        return result;
    }

    /// <summary>
    /// Slows rotation with maximum force, for use when stopping from speed.
    /// Stops from rotationSpeed in 5 FixedUpdates (1/10th of a second).
    /// Formula:  inertia * rotationspeed * deg2rad * 10.
    /// If you are less than formula, just use RotationStop()
    /// </summary>
    private void RotationHardSlow(){
        if (rb.angularVelocity > 0)
        {
            rb.AddTorque(-rb.inertia * rotationSpeed * Mathf.Deg2Rad * 10f);
        } else {
            rb.AddTorque(rb.inertia * rotationSpeed * Mathf.Deg2Rad * 10f);
        }
    }

    /// <summary>
    /// Slows rotation with minimal force, for use when spinning.
    /// Stops from rotationSpeed in 5 seconds.
    /// Formula:  inertia * rotationspeed * deg2rad / 5.
    /// If you are less than formula, just use RotationStop()
    /// </summary>
    private void RotationSoftSlow(){
        if (rb.angularVelocity > 0)
        {
            rb.AddTorque(-rb.inertia * rotationSpeed * Mathf.Deg2Rad / 5f);
        } else {
            rb.AddTorque(rb.inertia * rotationSpeed * Mathf.Deg2Rad / 5f);
        }
    }

    /// <summary>
    /// Stops all rotation immediately, for use when stopping from low speed.
    /// </summary>
    private void RotationStop(){
        rb.angularVelocity = 0;
    }

    /// <summary>
    /// Accelerates rotation by a given amount
    /// </summary>
    private void RotationAccelerate(float acceleration){
        rb.AddTorque(rb.inertia * acceleration * Mathf.Deg2Rad);
    }

    private void RotationSetToMax(bool left){
        float speedDiff;
        if (left) {
            speedDiff = rb.angularVelocity - rotationSpeed;
        } else {
            speedDiff = rb.angularVelocity + rotationSpeed;
        } 
        rb.AddTorque(-rb.inertia * speedDiff * Mathf.Deg2Rad * 50f);
    }



    private static float PickAcceleration(float velocity, float distance, float minAcc, float maxAcc){
        // Gas:  Most inline with current velocity
        // Brakes:  Most out of line with current velocity
        float gas, brakes;
        float minStopDistance;
        float minVelocity, maxVelocity, singleFrameVelocity;
        bool sFVCheck;
        if (velocity >= 0){
            gas = maxAcc;
            brakes = minAcc;
        } else {
            gas = minAcc;
            brakes = maxAcc;
        }

        minStopDistance = -velocity*velocity / (2*brakes);
        
                
        // If I can't stop in time, max brakes.
        if ((distance > 0 && minStopDistance >= distance) || (distance < 0 && minStopDistance <= distance)){
            return brakes;
        }



        // SOMETHING HERE IS BROKEN STILL :*(
        if (velocity >= 0){
            minVelocity = velocity + brakes * 0.02f;
            maxVelocity = velocity + gas * 0.02f;
        } else {
            minVelocity = velocity + gas * 0.02f;
            maxVelocity = velocity + brakes * 0.02f;
        }
        singleFrameVelocity = distance * 50f;
        sFVCheck = Mathf.Abs(singleFrameVelocity) < Mathf.Abs(brakes * 0.02f);
        // If I could get exactly there with enough brakes to stop immediately after, move to exactly that amount.
        if (minVelocity <= singleFrameVelocity && singleFrameVelocity <= maxVelocity && sFVCheck){
            Debug.Log(distance + ", " + velocity + ", " + (velocity - singleFrameVelocity));
            return (velocity - singleFrameVelocity)*10f;
        }
        
        // Still no?  Okay, then if velocity and distance are aligned, gas it.
        if ((velocity > 0 && distance > 0) || (velocity < 0 && distance < 0))
            return gas;

        // Uhoh, we're going the wrong way, brakes!
        return brakes;

    }

    /// <summary>
    /// A complete movement instruction.
    /// Generated on command, followed until completion/stopped.
    /// </summary>
    private class Instruction{
        private MoveState state = new MoveState();
        private Target moveTarget;
        private Target lookTarget;
        private Vector2 moveDirection;
        private Vector2 lookDirection;

        private SinglePointMovement parent;

        public Instruction(SinglePointMovement _parent){
            parent = _parent;
        }

        /// <summary>
        /// Makes the next movement.
        /// </summary>
        public void Follow(){
            if (lookTarget != null){
                lookDirection = new Vector2(lookTarget.TargetLocation().x - parent.transform.position.x, 
                                    lookTarget.TargetLocation().y - parent.transform.position.y);
            }

            // If we're trying to look at something, look at it.
            if (state.isLooking){
                float desiredDirection = Vector2.SignedAngle(new Vector2(0,1),lookDirection);
                float diff = desiredDirection - parent.GetAngle();
                if (diff < -180) diff += 360;
                if (diff > 180) diff -= 360;
                float velocity = parent.rb.angularVelocity;
                float minAcc = -parent.rotationSpeed*10f;
                float maxAcc = parent.rotationSpeed*10f;
                if (velocity > 0){
                    if (maxAcc * 0.02f + velocity > parent.rotationSpeed){
                        maxAcc = (parent.rotationSpeed - velocity) * 0.02f;
                    }
                } else {
                    if (minAcc * 0.02f + velocity < -parent.rotationSpeed){
                        minAcc = -(parent.rotationSpeed - velocity) * 0.02f;
                    }
                }
                float finalAcceleration = PickAcceleration(parent.rb.angularVelocity, diff, minAcc, maxAcc);
                parent.RotationAccelerate(finalAcceleration);
            }

            // Otherwise, if we are moving, look forward

            // And if not, just try to stop spinning
        }

        public void SetLookTarget(Target target){
            lookTarget = target;
            state.StartLooking();
        }
    }

    /// <summary>
    /// State variables for a given instruction
    /// </summary>
    private class MoveState{
        // Reflects only the current goal of the movement.
        // So if it gets hit but doesn't have an instruction,
        // isMoving could be false but velocity could be nonzero.
        public bool isMoving = false;
        public bool isLooking = false;
        public bool hasEndpoint = false;
        public bool isSliding = false;
        public bool isSpinning = false;
        

        public void StartSlide(){
            isSliding = true;
            isMoving = false;
        }

        public void Stop(){
            isMoving = false;
            isSliding = false;
            hasEndpoint = false;
        }

        public void StopLooking(){
            isLooking = false;
        }

        public void StartMoving(bool _hasEndpoint){
            isMoving = true;
            hasEndpoint = _hasEndpoint;
        }

        public void StartLooking(){
            isLooking = true;
        }
    }

    private class Target{
        private Vector2 vectorTarget;
        private Transform transformTarget;
        private bool isVector;

        public Target(Vector2 _vectorTarget){
            vectorTarget = _vectorTarget;
            isVector = true;
        }

        public Target(Transform _transformTarget){
            transformTarget = _transformTarget;
            isVector = false;
        }

        public Vector2 TargetLocation(){
            if (isVector){
                return new Vector2(vectorTarget.x, vectorTarget.y);
            } else {
                return new Vector2(transformTarget.position.x, transformTarget.position.y);
            }
        }
    }
}
