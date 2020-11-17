using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Driving : MonoBehaviour {
    [Header ("Physics Settings")]
    [Tooltip ("Top acceleration, in meters per second")]
    public float acceleration = 6;
    [Tooltip ("Standing (not sliding) friction, 0.9 is reality")]
    public float staticCoefficientOfFriction = 0.9f;
    [Tooltip ("Sliding friction, 0.5-0.8 is reality, less is more fun")]
    public float kineticCoefficientOfFriction = 0.5f;
    [Tooltip ("Distance from center of object to center of front axel (meters)")]
    public float frontAxleDistance = 2f;
    [Tooltip ("Distance from center of vehicle to center of rear axel (meters)")]
    public float rearAxleDistance = 1.5f;
    [Tooltip ("Boosting force, in Newtons.  Will not scale with car weight!")]
    public float boostPower = 10000f;
    private bool frontSliding = false;
    private bool rearSliding = false;
    
     GameController controller;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GameObject.Find("GameControllerObject").GetComponent<GameController>();
    }

    // Update is called once per frame
    void FixedUpdate () {
        float verticalInput = Input.GetAxis ("Vertical");
        float horizontalInput = Input.GetAxis ("Horizontal");
        float handbrake = Input.GetAxis ("Jump");
        float boost = Input.GetAxis("Boost");
        float wheeldirection = horizontalInput * Mathf.Rad2Deg; // +- 1 radian, ~60 degrees
        Quaternion rot = Quaternion.Euler (0, 0, -wheeldirection);
        Vector2 frontWheels = (Vector2) (rot * transform.up); // unit vector pointing at angle of front wheels
        Vector2 rearWheels = transform.up; // unit vector pointing at angle of rear wheels (straight forward)
        Rigidbody2D body = gameObject.GetComponent<Rigidbody2D> ();
        Player player = controller.GetPlayer();

        if (player.GetLevelStats().stats[LevelRewards.ConditionType.TopSpeed] < body.velocity.magnitude)
            player.GetLevelStats().SetStat(LevelRewards.ConditionType.TopSpeed, body.velocity.magnitude);

        if (verticalInput < 0)
            player.GetLevelStats().AddStat(LevelRewards.ConditionType.Brakes, 0.02f);
            
        // Add acceleration
        body.AddForceAtPosition (frontWheels * acceleration * body.mass * verticalInput, transform.position + transform.up * frontAxleDistance);

        // Add boost
        if (boost == 1 && controller.GetPlayer().currentNO2 > 0) {
            controller.GetPlayer().currentNO2 -= 1;
            if (controller.GetPlayer().currentNO2 < 0) controller.GetPlayer().currentNO2 = 0;

            body.AddForce(boostPower * transform.up);
        }

        // Compute axel velocities
        Vector2 fwv = body.velocity + frontAxleDistance * Mathf.Deg2Rad * body.angularVelocity * Vector2.Perpendicular (transform.up);
        Vector2 rwv = body.velocity - rearAxleDistance * Mathf.Deg2Rad * body.angularVelocity * Vector2.Perpendicular (transform.up);

        // Compute horizontal velocities
        Vector2 fwHorizontal = fwv - Vector2.Dot (fwv, frontWheels) * frontWheels;
        Vector2 rwHorizontal = rwv - Vector2.Dot (rwv, rearWheels) * rearWheels;

        if (handbrake == 1)
            rearSliding = true;
        else
            rearSliding = false;

        float maxStoppingPower;
        // For front wheel
        if (fwHorizontal.magnitude > 0) {
            if (frontSliding)
                maxStoppingPower = body.mass * kineticCoefficientOfFriction * 9.81f;
            else
                maxStoppingPower = body.mass * staticCoefficientOfFriction * 9.81f;
            if (fwHorizontal.magnitude * body.mass * 30 < maxStoppingPower)
                maxStoppingPower = fwHorizontal.magnitude * body.mass * 30;
            fwHorizontal *= -maxStoppingPower / fwHorizontal.magnitude;
            body.AddForceAtPosition (fwHorizontal, transform.position + transform.up * frontAxleDistance);
        }

        if (rwHorizontal.magnitude > 0) {
            if (rearSliding)
                maxStoppingPower = body.mass * kineticCoefficientOfFriction * 9.81f;
            else
                maxStoppingPower = body.mass * staticCoefficientOfFriction * 9.81f;
            if (rwHorizontal.magnitude * body.mass * 30 < maxStoppingPower)
                maxStoppingPower = rwHorizontal.magnitude * body.mass * 30;
            rwHorizontal *= -maxStoppingPower / rwHorizontal.magnitude;
            body.AddForceAtPosition (rwHorizontal, transform.position - transform.up * rearAxleDistance);
        }

    }

}