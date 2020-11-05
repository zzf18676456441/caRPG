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
    [Tooltip ("Braking, in meters per second^2 (much higher for humans than cars)")]
    public float braking = 20;
    [Tooltip ("Deceleration when sliding, in meters per second^2 (should be lower than braking)")]
    public float sliding = 2;
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
        instruction = new Instruction(this);
    }

    /// <summary>
    /// TAKES OVER RIGIDBODY DRAG PHYSICS
    /// </summary>
    void Start(){
        rb.drag = 0;
        rb.angularDrag = 0;
    }

    /// <summary>
    /// Follows whatever instruction every FixedUpdate (.02 seconds)
    /// </summary>
    void FixedUpdate(){
        instruction.Follow();
    }

    /// <summary>
    /// Moves along the desired vector.
    /// Continues moving until issued another command.
    /// </summary>
    /// <param name="desiredDirection">Direction to move in.</param>
    public void MoveByVector(Vector2 desiredDirection){
        instruction.SetMoveTarget(new Target(desiredDirection),false);
    }

    /// <summary>
    /// Chases the target transform.
    /// Continues moving until issued another command.
    /// </summary>
    /// <param name="target">Transform to chase</param>
    public void Chase(Transform target){
        instruction.SetMoveTarget(new Target(target),true);
    }

    /// <summary>
    /// Moves to a specific location, then stops.
    /// Continues moving until issued another command, or until destination is reached.
    /// </summary>
    /// <param name="location">Location to move to</param>
    public void MoveToLocation(Vector2 location){
        instruction.SetMoveTarget(new Target(location),true);
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
        instruction.SetLookTarget(new Target(location));
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
        instruction.StopMoving();
    }

    /// <summary>
    /// Releases any current looking commands, then resumes normal behavior.
    /// </summary>
    public void StopLooking(){
        instruction.StopLooking();
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
    /// The angle the gameObject is currently facing, taking into account the sprite direction.
    /// 0 degrees is up, Rotation is counter-clockwise (left).
    /// For example:  Facing right on the screen would be (-90) on the angle.
    /// </summary>
    /// <returns>The angle, in degrees, clamped to (-180,180)</returns>
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
    /// Accelerates rotation by a given amount
    /// </summary>
    private void RotationAccelerate(float acceleration){
        rb.AddTorque(rb.inertia * acceleration * Mathf.Deg2Rad);
    }

    /// <summary>
    /// Accelerates linear velocity by a given amount along the given vector
    /// </summary>
    private void LinearAccelerate(float acceleration, Vector2 direction){
        float dMag = direction.magnitude;
        if (dMag == 0) return;
        direction *= acceleration*rb.mass/dMag;
        rb.AddForce(direction);
    }





    /*
    *
    *     HELPER CLASSES  
    *
    */



    /// <summary>
    /// A complete movement instruction.
    /// Generated on command, followed until completion/stopped.
    /// </summary>
    private class Instruction{
        private MoveState state = new MoveState();
        private Target moveTarget;
        private Target lookTarget;

        private SinglePointMovement parent;

        public Instruction(SinglePointMovement _parent){
            parent = _parent;
        }

        /// <summary>
        /// Makes the next movement.
        /// </summary>
        public void Follow(){
            if(state.isIdle) return;
            float desiredDirection, diff, velocity, finalAcceleration;
            Vector2 lookDirection, moveDirection;
            Vector2 undesiredVelocity; // The amount of the current velocity
            FeasibleMoves fm;
            // If we're trying to look at something, look at it.
            if (state.isLooking){
                lookDirection = new Vector2(lookTarget.TargetLocation().x - parent.transform.position.x, 
                                    lookTarget.TargetLocation().y - parent.transform.position.y);
                desiredDirection = Vector2.SignedAngle(new Vector2(0,1),lookDirection);
            } else if (state.isMoving){
                desiredDirection = Vector2.SignedAngle(new Vector2(0,1),parent.rb.velocity);
            } else {
                desiredDirection = parent.GetAngle();
            }
            diff = desiredDirection - parent.GetAngle();
            if (diff < -180) diff += 360;
            if (diff > 180) diff -= 360;
            //Debug.Log("Diff: " + diff + ", aVel: " + parent.rb.angularVelocity + ", looking: " + state.isLooking);
            fm = new FeasibleMoves(diff,parent.rb.angularVelocity,parent.rotationSpeed,parent.rotationSpeed*10f,parent.rotationSpeed*10f,parent.rotationSpeed/5f);
            finalAcceleration = fm.PickAcceleration();
            parent.RotationAccelerate(finalAcceleration);
            
            if (state.isMoving){
                if (state.hasEndpoint){
                    moveDirection = new Vector2(moveTarget.TargetLocation().x - parent.transform.position.x, 
                                    moveTarget.TargetLocation().y - parent.transform.position.y);
                } else {
                    moveDirection = moveTarget.TargetLocation();
                }
                diff = moveDirection.magnitude;
                if (diff > 0) {
                    moveDirection.Normalize();
                    velocity = Vector2.Dot(parent.rb.velocity,moveDirection);
                    undesiredVelocity = parent.rb.velocity - velocity*moveDirection;
                    //Debug.Log("Distance: " + diff + ", Velocity: " + velocity);
                    fm = new FeasibleMoves(diff,velocity,parent.maxSpeed,parent.acceleration,parent.braking,parent.sliding);
                    finalAcceleration = fm.PickAcceleration();
                    
                    parent.LinearAccelerate(finalAcceleration, moveDirection);
                } else {
                    undesiredVelocity = parent.rb.velocity;
                }
            } else {
                undesiredVelocity = parent.rb.velocity;
            }
            if (undesiredVelocity.magnitude > 0) {
                fm = new FeasibleMoves(0,undesiredVelocity.magnitude,parent.maxSpeed,parent.acceleration,parent.braking,parent.braking/5f);
                finalAcceleration = fm.PickAcceleration();
                parent.LinearAccelerate(finalAcceleration, undesiredVelocity);
            }
            
        }

        public void SetLookTarget(Target target){
            lookTarget = target;
            state.StartLooking();
        }

        public void SetMoveTarget(Target target, bool stopThere){
            moveTarget = target;
            state.StartMoving(stopThere);
        }

        public void StopMoving(){
            state.Stop();
        }

        public void StopLooking(){
            state.StopLooking();
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
        public bool isIdle = true;
        

        public void StartSlide(){
            isSliding = true;
            isMoving = false;
            isIdle = false;
        }

        public void Stop(){
            isMoving = false;
            isSliding = false;
            hasEndpoint = false;
            isIdle = false;
        }

        public void StopLooking(){
            isLooking = false;
            isIdle = false;
        }

        public void StartMoving(bool _hasEndpoint){
            isMoving = true;
            hasEndpoint = _hasEndpoint;
            isIdle = false;
        }

        public void StartLooking(){
            isLooking = true;
            isIdle = false;
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

    private class FeasibleMoves{
        // Goals, derived
        public float velocity, gas, brakes, distance, direction, minPV, maxPV;
        // Velocity (signed) - current speed and direction
        // Distance (unsigned) - distance to goal (always positive)
        // Direction (signed) -  +/- 1, depending if this is all backwards
        // Gas (signed) - maximum acceleration towards direction.  Usually positive, but could be negative if velocity is too high.
        // Brakes (signed) - usually negative, but could be postive if velocity is negative.  Always smaller than gas.
        // minPV - slowest towards the goal
        // maxPV - fastest towards the goal


        /// <summary>
        /// 
        /// </summary>
        /// <param name="distance">(SIGNED) Distance to target</param>
        /// <param name="velocity">(SIGNED) Current velocity</param>
        /// <param name="maxSpeed">(UNSIGNED) Max speed</param>
        /// <param name="maxAcceleration">(UNSIGNED) Max Acceleration</param>
        /// <param name="maxBraking">(UNSIGNED) Max Braking</param>
        /// <param name="minBraking">(UNSIGNED) Min Braking - think sliding friction</param>
        public FeasibleMoves(float distance, float velocity, float maxSpeed, float maxAcceleration, float maxBraking, float minBraking){
            if (distance >= 0) {
                direction = 1;
                this.velocity = velocity;
                this.distance = distance;
            } else {
                direction = -1;
                this.velocity = -velocity;
                this.distance = -distance;
            }
            // If |velocity| > maxSpeed, you must slow down (acts against velocity).
            if (this.velocity > maxSpeed) {
                gas = -minBraking;
                brakes = -maxBraking;
            } else if (this.velocity < -maxSpeed) {
                gas = maxBraking;
                brakes = minBraking;
            } else {
                // Okay you can accelerate or brake, up to you
                if (this.velocity >= 0){
                    gas = maxAcceleration;
                    brakes = -maxBraking;
                } else {
                    gas = maxBraking;
                    brakes = minBraking;
                }
            }
            
            minPV = this.velocity + brakes * 0.02f;
            maxPV = this.velocity + gas * 0.02f;
            if (minPV > maxPV) Debug.Log("Error in FM: range is flipped");
        }

        public float PickAcceleration(){
            float goalPV;
            // Case 0:  Am I already there?
            if (distance == 0) {
               // Debug.Log("Case 0");
                // If so, can I stop this frame?
                if(minPV < 0 && maxPV > 0){
                    return -direction*velocity*50f;
                }
                // If not, at least slow down as much as possible
                if(velocity > 0) return direction*brakes;
                else return direction*gas;
            }

            // Case 1:  Am I going the wrong way?
            if (velocity < 0) {
                //Debug.Log("Case 1");
                // Would I overshoot if I went to max velocity?
                if(maxPV > 0 && maxPV > distance * 50f){
                    return direction * (distance*50f - velocity) * 50f;
                }
                // Nope, gas it
                return direction * gas;
            }
            
            // At this point we know distance > 0 && velocity >= 0 
            // Next check: If I slam the brakes right now, how far will I go?
            int fastestStop = (int) (-50f * velocity / brakes);  // In frames, 50 frames/second
            float brakingDistance = (fastestStop * velocity * 0.02f + fastestStop * (fastestStop + 1) * brakes * 0.0002f);
            // Case 2:  I can't stop, but might as well try
            if (brakingDistance >= distance){
                //Debug.Log("Case 2");
                return direction*brakes;
            }
            // Case 3:  I could stop next frame, can I get far enough?
            if (fastestStop <= 1){
                goalPV = distance*50f;
                if(minPV < goalPV && goalPV < maxPV){
                    //Debug.Log("Case 3");
                    //Debug.Log(velocity + "/" + distance + "/" + brakes);
                    return direction * (goalPV - velocity) * 50f;
                }
            }

            // Okay, what if I gassed it for a frame, then braked as hard as possible?
            fastestStop = (int) (-50f * maxPV / brakes);  // In frames, 50 frames/second
            brakingDistance = maxPV * 0.02f + (fastestStop * maxPV * 0.02f + fastestStop * (fastestStop + 1) * brakes * 0.0002f);
            //if(fastestStop == 1) Debug.Log("STOP NEXT FRAME!!!!!");
            // Case 4: I'd still be fine after gassing it
            if (brakingDistance < distance){
                //Debug.Log("Case 4");
                return direction*gas;
            }

            // Next Question:  How fast can I go this frame and still stop at the right spot?
            goalPV = (distance - fastestStop * (fastestStop + 1) * brakes * .01f) / (1 + fastestStop);
            if (goalPV < maxPV && goalPV > minPV) {
                //Debug.Log("Case 5");
                //Debug.Log(goalPV + "/" + distance + "/" + fastestStop + "/" + brakes);
                return direction * (goalPV - velocity) * 50f;
            }

            // Catch incase something weird happened
            if (goalPV > maxPV){
                //Debug.Log("Gas it!");
                return direction*gas;
            }
            // Case 4:  TODO - fastestStop = 1!
            return direction*brakes;
        }
    }


}
