using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using TMPro;

public class FordGT_Move_player : MonoBehaviour
{
    public TextMeshProUGUI speedoMeter;
    public RawImage rpmNeedle;
    GameObject[] wheels;

    float angle = 0f;
    float rotateBack = 0f;
    float wheelDirection = 1;


    float strenght = 50.0f;
    Rigidbody rigidBody;

    float totalRot = 0f;

    float needleAngle = 0;

    //RealisticEngineSound_mobile engineScript = transform.Find("v8_italian").gameObject.GetComponent<RealisticEngineSound_mobile>();
    //Car myCar = new Car(engineScript);
    Car myCar;

    // Start is called before the first frame update
    void Start()
    {
        //myResults = otherGameObject.GetComponentInParent<ComponentType>()

        //myCar = new Car(140, 7000, 40, 1550, 0, 2);
        //foreach (var wheel in wheels) { 
        //    wheel.transform.rotation = startRotation;
        //}
        rigidBody = GetComponent<Rigidbody>();


        speedoMeter = GameObject.Find("SpeedoMeter").GetComponent<TextMeshProUGUI>();
        rpmNeedle = GameObject.Find("RPMNeedle").GetComponent<RawImage>();


        RealisticEngineSound_mobile engineScript = transform.Find("V8_american_modern").gameObject.GetComponent<RealisticEngineSound_mobile>();
        myCar = new Car(engineScript);
    }

    // Update is called once per frame
    void Update()
    {
        var gamepad = Gamepad.current;
        if (gamepad == null)
        {
            Debug.Log("controller not connected");
            return;
        }
        float horizontal = gamepad.leftStick.x.ReadValue();
        float vertical = gamepad.rightTrigger.ReadValue() - gamepad.leftTrigger.ReadValue();

        // Loop car acceleration
        myCar.accelerate(vertical);

        // RPM GAUGE AND SPEEDOMETER
        speedoMeter.SetText("   G" + myCar.gearBox.currentGear + "\n\n" + myCar.displaySpeedInKM + "kmh");

        int maxRPM = 8000;
        int maxDeg = 260;

        needleAngle = 220 - 260 * (myCar.currentRPM / maxRPM);
        rpmNeedle.transform.eulerAngles = new Vector3(0, 0, needleAngle);  // rpmNeedle.transform.eulerAngles.z + 1

        Transform fordGT = transform.Find("_2017_Ford_GT");
        GameObject[] frontWheels = { fordGT.Find("gum.022").gameObject, fordGT.Find("gum.021").gameObject };
        GameObject[] backWheels = { fordGT.Find("gum.024").gameObject, fordGT.Find("gum.023").gameObject };

        if (myCar.currentSpeed < -1)
        {
            wheelDirection = -1;
        }
        else if (myCar.currentSpeed > 1)
        {
            wheelDirection = 1;
        }

        // Front Wheels
        totalRot -= myCar.wheelRPM * Time.deltaTime * 5 * wheelDirection;
        foreach (var wheel in frontWheels)
        {
            wheel.transform.eulerAngles = new Vector3(transform.eulerAngles.x - totalRot, transform.eulerAngles.y + (horizontal * myCar.maxWheelTurnAngle), transform.eulerAngles.z);
        }

        // Back Wheels
        foreach (var wheel in backWheels)
        {
            wheel.transform.Rotate(new Vector3(-myCar.wheelRPM, 0, 0) * Time.deltaTime * 10);
        }

        // Car movement Translate
        transform.Translate(new Vector3(0, 0, myCar.currentSpeed / 20) * Time.deltaTime); // myCar.currentSpeed /40
        // Car Rotate
        if (myCar.currentSpeed > 0)
        {
            transform.Rotate(new Vector3(0, horizontal / 30, 0) * Time.deltaTime * 1000);
        }
        else if (myCar.currentSpeed < 0)
        {
            transform.Rotate(new Vector3(0, -horizontal / 30, 0) * Time.deltaTime * 1000);
        }

        myCar.isManual = true;
        myCar.engineScript.isShifting = false;
        // Gear Shifting
        /*if ((Input.GetKeyDown("q") || gamepad.leftShoulder.wasPressedThisFrame) && myCar.gearBox.currentGear > 1)
        {
            myCar.engineScript.isShifting = true;
            myCar.manualShiftDown();
        }
        else if ((Input.GetKeyDown("e") || gamepad.rightShoulder.wasPressedThisFrame) && myCar.gearBox.currentGear < myCar.gearBox.gearCount)
        {
            myCar.engineScript.isShifting = true;
            myCar.manualShiftUp();
        }*/

        XboxControlls controller = new XboxControlls(myCar.gearBox);
        if (controller.upShift())
        {
            myCar.manualShiftUp();
        }
        else if (controller.downShift())
        {
            myCar.manualShiftDown();
        }
    }
}
