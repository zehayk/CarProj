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
    public Text PointsText;
    private bool enabledMotor = true;
    private float points = 0f;
    private float startTime;

    public Text gameOverText;
    // Center of mass
    public Vector3 com;
    public Rigidbody rb;
    public GearBox gearBox = new GearBox();

    public RealisticEngineSound_mobile CarEngine;

    void Start()
    {
        startTime = Time.time;
        enabledMotor = true;
        Controls = new basicController();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;

        mainCamera = Camera.main;
        mainCamera.transform.localPosition = new Vector3(0f, 2.5f, -4.75f);
    }

    private void Update()
    {
        float speed = rb.velocity.magnitude;
        float currentTime = Time.time;
        float timeDifference = currentTime - startTime;
        points += ((timeDifference * speed)/1000);
        PointsText.text = points.ToString("F2") + "$";

        if (Controls.upShift())
        {
            Debug.Log("waaa");
            gearBox.upShift();
        }
        else if (Controls.downShift())
        {
            gearBox.downShift();
        }
    }

    void FixedUpdate()
    {
        Debug.Log(Input.GetKeyDown(KeyCode.Space));
        float speed = rb.velocity.magnitude;
        if (enabledMotor)
        {
            float motor = maxMotorTorque * Controls.throttle() * ((gearBox.currentGear < 1) ? -1 : 1);
            // float brakes = maxBrakeTorque * Controls.leftShoulder();
            float brakes = maxBrakeTorque * Controls.brake();
            float handBrake = maxHandBrakeTorque * Controls.handbrake();
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
                    if (currentSpeed <= gearBox.maxSpeed())
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

        speedText.text = "Speed: " + (speed * 3.6f).ToString("F0") + " Km/h" + " G: " + ((gearBox.currentGear < 1) ? "R" : gearBox.currentGear);

        // Engine Red Line 7000
        CarEngine.engineCurrentRPM = (speed / gearBox.maxSpeed()) * CarEngine.maxRPMLimit;
    }


    public GameObject explosion;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Enemy"))
        {
            enabledMotor = false;
            Instantiate(explosion, gameObject.transform);
            gameOverText.text = "GAME OVER\nYOU CRASHED\n"+points+ "$ WON";
            Invoke("SwitchToNextScene", 5f);
        }
        else if (collision.collider.CompareTag("EnemyCop"))
        {
            enabledMotor = false;
            Instantiate(explosion, gameObject.transform);
            gameOverText.text = "GAME OVER\nCOP CAUGHT YOU\n" + points + "$ WON";
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

    // public float rightMinusLeftTrig() => (isPresent()) ? gamepad.rightTrigger.ReadValue() - gamepad.leftTrigger.ReadValue() : Input.GetAxis("Vertical");
    public float throttle() => (isPresent()) ? gamepad.rightTrigger.ReadValue() : ((Input.GetAxis("Vertical") > 0) ? Input.GetAxis("Vertical") : 0);
    public float brake() => (isPresent()) ? gamepad.leftTrigger.ReadValue() : ((Input.GetAxis("Vertical") < 0) ? -1 * Input.GetAxis("Vertical") : 0);
    public float leftStick() => (isPresent()) ? gamepad.leftStick.x.ReadValue() : Input.GetAxis("Horizontal");
    // public int leftShoulder() => ((isPresent()) ? gamepad.leftShoulder.isPressed : Input.GetKey(KeyCode.Space)) ? 1 : 0;
    public bool downShift() => (isPresent()) ? gamepad.leftShoulder.wasPressedThisFrame : Input.GetKeyDown("q");
    public bool upShift() => (isPresent()) ? gamepad.rightShoulder.wasPressedThisFrame : Input.GetKeyDown("e");
    public float[] rightStick() => new float[] { (isPresent()) ? gamepad.rightStick.x.ReadValue() : Input.GetAxis("Mouse X"), (isPresent()) ? gamepad.rightStick.y.ReadValue() : Input.GetAxis("Mouse Y") };
    public int handbrake() => (isPresent() ? (gamepad.buttonWest.isPressed ? 1 : 0) : (Input.GetKeyDown("space") ? 1 : 0));
}

public class GearBox 
{
    public int currentGear = 1;
    public int maxGears = 6;
    public GearBox()
    {
    }

    public void upShift() 
    {
        if (this.currentGear < this.maxGears)
        {
            this.currentGear++;
        } 
    }

    public void downShift() 
    {
        if (this.currentGear > 0)
        {
            this.currentGear--;
        }
    }

    public float maxSpeed() 
    {
        switch (currentGear)
        {
            case 0: // this is reverse
                return 20;
            case 1:
                return 15;
            case 2:
                return 30;
            case 3:
                return 45;
            case 4:
                return 60;
            case 5:
                return 75;
            case 6:
                return 90;
            default:
                return 0;
        }
    }
}