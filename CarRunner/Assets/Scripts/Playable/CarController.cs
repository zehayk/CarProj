using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to the wheel
    public float maxBrakeTorque; // maximum torque the motor can apply to the wheel
    public float maxHandBrakeTorque; // maximum torque the motor can apply to the wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    public float minCameraDistance = 5f; // Minimum camera distance
    public float maxCameraDistance = 20f; // Maximum camera distance
    public float cameraDistanceMultiplier = 0.1f; // Multiplier for camera distance based on speed
    private basicController Controls;
    private Camera mainCamera;

    // Center of mass
    public Vector3 com;
    public Rigidbody rb;

    void Start()
    {
        Controls = new basicController();
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;

        // Assuming the main camera is a child of the player
        mainCamera = Camera.main;
    }

    void FixedUpdate()
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
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            axleInfo.leftWheel.brakeTorque = brakes;
            axleInfo.rightWheel.brakeTorque = brakes;
        }

        axleInfos[0].rightWheel.brakeTorque = handBrake;
        axleInfos[0].leftWheel.brakeTorque = handBrake;

        // Adjust camera distance based on speed
        float speed = rb.velocity.magnitude;
        float cameraDistance = Mathf.Lerp(minCameraDistance, maxCameraDistance, speed * cameraDistanceMultiplier);
        mainCamera.transform.localPosition = new Vector3(0f, 0f, -cameraDistance);
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



/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
    
public class CarController : MonoBehaviour {

    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxBrakeTorque; // maximum torque the motor can apply to wheel
    public float maxHandBrakeTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    private basicController Controls;

    // Center of masss
    public Vector3 com;
    public Rigidbody rb;

    void Start() {
        Controls = new basicController();

        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = com;
    }   
    void FixedUpdate()
    {
        float motor = maxMotorTorque * Controls.rightMinusLeftTrig();
        float brakes = maxBrakeTorque * Controls.leftShoulder();
        float handBrake = maxHandBrakeTorque * Controls.westbutton();
        float steering = maxSteeringAngle * Controls.leftStick();


        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                    
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            axleInfo.leftWheel.brakeTorque = brakes;
            axleInfo.rightWheel.brakeTorque = brakes;
        }

        axleInfos[0].rightWheel.brakeTorque = handBrake;
        axleInfos[0].leftWheel.brakeTorque = handBrake;
        Debug.Log(Controls.isPresent());
    }
}
    
[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class basicController {
    private ButtonControl _rightTrig = Gamepad.current.rightTrigger;
    private ButtonControl _leftTrig = Gamepad.current.leftTrigger;

    private ButtonControl _leftShoulder = Gamepad.current.leftShoulder;
    private ButtonControl _rightShoulder = Gamepad.current.rightShoulder;

    private ButtonControl ButtonWest = Gamepad.current.buttonWest;

    private StickControl _leftStick = Gamepad.current.leftStick;
    private StickControl _rightStick = Gamepad.current.rightStick;

    public bool isPresent() => Gamepad.current != null;
    // R2
    public float rightMinusLeftTrig() => (isPresent()) ? _rightTrig.ReadValue() - _leftTrig.ReadValue() : Input.GetAxis("Vertical");
    // L3
    public float leftStick() => (isPresent()) ? _leftStick.x.ReadValue() : Input.GetAxis("Horizontal");
    // L2

    public int leftShoulder() => ((isPresent()) ? _leftShoulder.isPressed : Input.GetKey(KeyCode.Space)) ? 1 : 0;
    // public bool downShift() => ((_downShiftKey || _downShiftBind.wasPressedThisFrame) && _gearboxObj.currentGear > 1);
    // public bool upShift() => ((_upShiftKey || _upShiftBind.wasPressedThisFrame) && _gearboxObj.currentGear < _gearboxObj.gearCount);
    public float[] rightStick() => new float[] { _rightStick.x.ReadValue(), _rightStick.y.ReadValue() };

    // X / Square
    public int westbutton() => (isPresent() && ButtonWest.isPressed) ? 1 : 0;

    public basicController()
    {
    }
}*/