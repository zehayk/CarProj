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
    public float maxSteeringAngle; // maximum steer angle the wheel can have
    private basicController Controls;
    void Start() {
        Controls = new basicController();
    }   
    void FixedUpdate()
    {
        float motor = maxMotorTorque * Controls.vertical();
        float steering = maxSteeringAngle * Controls.horizontal();

        foreach (AxleInfo axleInfo in axleInfos) {
            if (axleInfo.steering) {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor) {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
                Debug.Log(axleInfo.leftWheel.motorTorque);
            }
        }
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
    private ButtonControl _throttleBind = Gamepad.current.rightTrigger;
    private ButtonControl _reverseBind = Gamepad.current.leftTrigger;
    private ButtonControl _downShiftBind = Gamepad.current.leftShoulder;
    private ButtonControl _upShiftBind = Gamepad.current.rightShoulder;
    private StickControl _steerStick = Gamepad.current.leftStick;
    private StickControl _freeLookStick = Gamepad.current.rightStick;
    public bool isPresent() => Gamepad.current != null;
    public float vertical() => (isPresent()) ? _throttleBind.ReadValue() - _reverseBind.ReadValue() : Input.GetAxis("Vertical");
    public float horizontal() => (isPresent()) ? _steerStick.x.ReadValue() : Input.GetAxis("Horizontal");

/*     public bool downShift() => ((_downShiftKey || _downShiftBind.wasPressedThisFrame) && _gearboxObj.currentGear > 1);
    public bool upShift() => ((_upShiftKey || _upShiftBind.wasPressedThisFrame) && _gearboxObj.currentGear < _gearboxObj.gearCount); */
    public float[] freeLookCoords() => new float[] { _freeLookStick.x.ReadValue(), _freeLookStick.y.ReadValue() };

    public basicController()
    {
    }
}