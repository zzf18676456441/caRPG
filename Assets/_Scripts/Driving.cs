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
    private bool frontSliding = false;
    private bool rearSliding = false;
    private int kills = 0;

    // Start is called before the first frame update
    void Start () {

    }

    // Update is called once per frame
    void FixedUpdate () {
        float verticalInput = Input.GetAxis ("Vertical");
        float horizontalInput = Input.GetAxis ("Horizontal");
        float handbrake = Input.GetAxis ("Jump");
        float wheeldirection = horizontalInput * Mathf.Rad2Deg / 2; // +- .5 radian, ~30 degrees
        Quaternion rot = Quaternion.Euler (0, 0, -wheeldirection);
        Vector2 frontWheels = (Vector2) (rot * transform.up); // unit vector pointing at angle of front wheels
        Vector2 rearWheels = transform.up; // unit vector pointing at angle of rear wheels (straight forward)
        Rigidbody2D body = gameObject.GetComponent<Rigidbody2D> ();

        // Add acceleration
        body.AddForceAtPosition (frontWheels * acceleration * body.mass * verticalInput, transform.position + transform.up * frontAxleDistance);

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

    void OnCollisionEnter2D (Collision2D col) {
        //debugText.GetComponent<UnityEngine.UI.Text> ().text = col.relativeVelocity.ToString ();
    }
}