using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class drifty_movement : MonoBehaviour
/*{
    public float speed = 30f;            // Car's maximum speed.
    public float rotationSpeed = 1f;   // Car's steering sensitivity.
    public float brakeForce = 10f;       // Brake force.

    float horizontalInput;
    float verticalInput;

    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        // Get input for movement.
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Calculate forward and backward force.
        Vector3 forwardForce = transform.forward * verticalInput * speed;

        // Apply force to move the car.
        rb.AddForce(forwardForce);

        // Calculate torque for steering.
        float rotationTorque = horizontalInput * rotationSpeed;

        // Apply torque to steer the car.
        rb.AddTorque(transform.up * rotationTorque);
        Debug.Log(transform.up);

        // Implement braking by applying reverse force when the brake key is pressed.
        if (Input.GetKey(KeyCode.Space))
        {
            rb.AddForce(-rb.velocity.normalized * brakeForce);
        }
    }
}*/
/*{
    public float speed = 1000000;              // Car's maximum speed.
    public float rotationSpeed = 30f;     // Car's steering sensitivity.
    public float driftThreshold = 20f;     // Angle at which the car starts to drift.
    public float driftForce = 10f;         // Force applied for drifting.

    private Rigidbody rb;
    private float horizontalInput;
    float verticalInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Get input for steering.
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        // Calculate the current turning angle.
        float currentAngle = rb.angularVelocity.magnitude; // Vector3.Angle(Vector3.forward, rb.velocity);

        // Calculate forward force.
        Vector3 forwardForce = transform.forward * speed;

        // Apply force to move the car.

        // Check if the current angle exceeds the drift threshold.
        if (currentAngle > driftThreshold)
        {
            Debug.Log("AAAAAAA");
            // Calculate the direction of the drift (left or right).
            float driftDirection = Mathf.Sign(horizontalInput);

            // Apply a sideways force to simulate drifting.
            Vector3 driftForceVector = transform.right * driftForce * driftDirection;
            rb.AddForce(driftForceVector, ForceMode.Impulse);
        }
        else
        {
            Debug.Log(transform.forward * speed * verticalInput);
            rb.AddForce(transform.forward * speed * verticalInput);

            // Apply regular steering force.
            rb.AddTorque(transform.up * horizontalInput * rotationSpeed);
        }

        if (Input.GetButton("Jump"))
        {
            //Apply a force to this Rigidbody in direction of this GameObjects up axis
            rb.AddForce(transform.up * 20f);
        }
    }
}*/

{
    public float maxSpeed = 100f;
    public float torque = 200f;
    public float driftFactor = 0.9f;
    public float steerSpeed = 2.0f;
    public float driftTorque = 2.5f;

    private Rigidbody rb;
    private float currentSpeed;
    private bool isDrifting = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // Get input for steering and acceleration
        float steerInput = Input.GetAxis("Horizontal");
        float accelerationInput = Input.GetAxis("Vertical");

        // Calculate steering angle
        float steerAngle = steerInput * steerSpeed;

        // Apply steering
        Quaternion turnAngle = Quaternion.Euler(0, steerAngle, 0);
        rb.MoveRotation(rb.rotation * turnAngle);

        // Apply acceleration
        currentSpeed = Vector3.Dot(rb.velocity, transform.forward);
        if (currentSpeed < maxSpeed)
        {
            rb.AddForce(transform.forward * accelerationInput * torque);
        }

        // Apply drifting effect
        if (Mathf.Abs(steerInput) > 0.9f && currentSpeed > 25f)
        {
            Debug.Log("aa");
            isDrifting = true;
            rb.AddTorque(transform.up * steerInput * driftTorque);
        }
        else
        {
            isDrifting = false;
        }

        // Adjust drifting factor
        if (isDrifting)
        {
        //    rb.velocity = rb.velocity * driftFactor;
        }
    }
}