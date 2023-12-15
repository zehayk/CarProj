using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to the wheel
    public float maxBrakeTorque; // maximum torque the motor can apply to the wheel
    public float maxHandBrakeTorque; // maximum torque the motor can apply to the wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public Vector3 startCamDistance = new Vector3(0, 2.5f, -4.75f); // Minimum camera distance
    public Vector3  endCameraDistance = new Vector3(0, 3.75f, -7.125f); // Maximum camera distance
    public float cameraDistanceMultiplier = 0.25f; // Multiplier for camera distance based on speed
    private basicController Controls;
    private Camera mainCamera;
    public float MaxSpeed = 30f;
    public Text speedText;
    private bool enabledMotor = true;

    public Text gameOverText;
    // Center of mass
    public Vector3 com;
    public Rigidbody rb;

    void Start()
    {
        enabledMotor = true;
        Controls = new basicController();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;

        mainCamera = Camera.main;
        mainCamera.transform.localPosition = new Vector3(0f, 2.5f, -4.75f);
    }

    void FixedUpdate()
    {
        float speed = rb.velocity.magnitude;
        if (enabledMotor)
        {
            float motor = maxMotorTorque * Controls.rightMinusLeftTrig();
            float brakes = maxBrakeTorque * Controls.leftShoulder();
            float handBrake = maxHandBrakeTorque * Controls.westbutton();
            float steering = maxSteeringAngle * Controls.leftStick();

            foreach (AxleInfo axleInfo in axleInfos)
            {
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = steering;
                    axleInfo.rightWheel.steerAngle = steering;
                }

                if (axleInfo.motor)
                {
                    float currentSpeed = rb.velocity.magnitude;
                    if (currentSpeed <= MaxSpeed)
                    {
                        axleInfo.leftWheel.motorTorque = motor;
                        axleInfo.rightWheel.motorTorque = motor;
                    }
                    else
                    {
                        axleInfo.leftWheel.motorTorque = 0;
                        axleInfo.rightWheel.motorTorque = 0;
                    }
                }

                axleInfo.leftWheel.brakeTorque = brakes;
                axleInfo.rightWheel.brakeTorque = brakes;
            }

            axleInfos[0].rightWheel.brakeTorque = handBrake;
            axleInfos[0].leftWheel.brakeTorque = handBrake;

        }
        Vector3 cameraDistance = Vector3.Lerp(startCamDistance, endCameraDistance, speed * cameraDistanceMultiplier);
        mainCamera.transform.localPosition = cameraDistance;

        speedText.text = "Speed: " + speed.ToString("F2") + " m/s";
        
    }


    public GameObject explosion;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            enabledMotor = false;
            Instantiate(explosion, gameObject.transform);
            gameOverText.text = "GAME OVER\nYOU CRASHED";
            Invoke("SwitchToNextScene", 5f);
        }
        else if (collision.collider.CompareTag("EnemyCop"))
        {
            enabledMotor = false;
            Instantiate(explosion, gameObject.transform);
            gameOverText.text = "GAME OVER\nCOP CAUGHT YOU";
            Invoke("SwitchToNextScene", 5f);
        }
    }
    private void SwitchToNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class basicController
{
    private Gamepad gamepad;

    public basicController()
    {
        // Check if a Gamepad is connected
        if (Gamepad.current != null)
        {
            gamepad = Gamepad.current;
        }
        else
        {
        }
    }

    public bool isPresent() => gamepad != null;

    public float rightMinusLeftTrig() => (isPresent()) ? gamepad.rightTrigger.ReadValue() - gamepad.leftTrigger.ReadValue() : Input.GetAxis("Vertical");
    public float leftStick() => (isPresent()) ? gamepad.leftStick.x.ReadValue() : Input.GetAxis("Horizontal");
    public int leftShoulder() => ((isPresent()) ? gamepad.leftShoulder.isPressed : Input.GetKey(KeyCode.Space)) ? 1 : 0;
    public float[] rightStick() => new float[] { (isPresent()) ? gamepad.rightStick.x.ReadValue() : Input.GetAxis("Mouse X"), (isPresent()) ? gamepad.rightStick.y.ReadValue() : Input.GetAxis("Mouse Y") };
    public int westbutton() => (isPresent() && gamepad.buttonWest.isPressed) ? 1 : 0;
}