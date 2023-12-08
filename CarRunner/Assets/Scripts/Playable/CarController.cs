using System;
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
        float motor = maxMotorTorque * Controls.rightTrig();
        float brakes = maxBrakeTorque * Controls.leftShoulder();
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

        axleInfos[0].rightWheel.brakeTorque = Controls.westbutton() * 1000000;
        axleInfos[0].leftWheel.brakeTorque = Controls.westbutton() * 1000000;
    }
}
    
[System.Serializable]
public class AxleInfo {
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
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
    public float rightTrig() => (isPresent()) ? _rightTrig.ReadValue() - _leftTrig.ReadValue() : Input.GetAxis("Vertical");
    // L3
    public float leftStick() => (isPresent()) ? _leftStick.x.ReadValue() : Input.GetAxis("Horizontal");
    // L2
    public int leftShoulder() => (isPresent() && _leftShoulder.isPressed) ? 1 : 0;
    // public bool downShift() => ((_downShiftKey || _downShiftBind.wasPressedThisFrame) && _gearboxObj.currentGear > 1);
    // public bool upShift() => ((_upShiftKey || _upShiftBind.wasPressedThisFrame) && _gearboxObj.currentGear < _gearboxObj.gearCount);
    public float[] rightStick() => new float[] { _rightStick.x.ReadValue(), _rightStick.y.ReadValue() };

    // X / Square
    public int westbutton() => (isPresent() && ButtonWest.isPressed) ? 1 : 0;

    public basicController()
    {
    }
}