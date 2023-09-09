using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;
using TMPro;


//2815692_Mclaren+Senna
[Serializable]
public class move : MonoBehaviour
{
    public TextMeshProUGUI speedoMeter;
    public RawImage rpmNeedle;
    GameObject[] wheels;
    Quaternion startRotation;
    float angle = 0f;
    float rotateBack = 0f;
    //float rotSpeed = 50;


    float strenght = 50.0f;
    Rigidbody rigidBody;

    float needleAngle = 0;

    Car myCar = new Car();

    // Start is called before the first frame update
    void Start()
    {

        //myResults = otherGameObject.GetComponentInParent<ComponentType>()

        myCar = new Car();

        //myCar = new Car(140, 7000, 40, 1550, 0, 2);
        wheels = GameObject.FindGameObjectsWithTag("FrontWheels");
        //foreach (var wheel in wheels) { 
        //    wheel.transform.rotation = startRotation;
        //}
        rigidBody = GetComponent<Rigidbody>();


        speedoMeter = GameObject.Find("SpeedoMeter").GetComponent<TextMeshProUGUI>();
        rpmNeedle = GameObject.Find("RPMNeedle").GetComponent<RawImage>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = Input.GetAxis("Vertical"); // forward backward
        float horizontal = Input.GetAxis("Horizontal");

        //Debug.Log(vertical);
        //if (myCar == null)
            //return;

        myCar.accelerate(vertical);


        //Debug.Log(horizontal);
        //Debug.Log(vertical);

        //Debug.Log(myCar.currentSpeed);


        //float a = Math.Floor(myCar.currentRPM / 10);
        //float speedInKM = (float)Math.Round(myCar.currentSpeed / 10, 1);
        //float ratio = 2.19f;
        // double a = Math.Round(((((myCar.currentSpeed / 60) * ratio) / myCar.tireCircumInMeter)) / myCar.finalDriveRatio, 2);


        // speedoMeter.SetText("G" + myCar.gearBox.currentGear + "\n" + myCar.currentRPM + " RPM\n" + myCar.speedInKM + "km/h\nstr: " + myCar.getAccelerationStrenght());
        speedoMeter.SetText("   G" + myCar.gearBox.currentGear + "\n\n" + myCar.speedInKM + "kmh");
        // rpmNeedle.transform.eulerAngles = new Vector3(0, 0, 220);  // rpmNeedle.transform.eulerAngles.z + 1
        
        int maxRPM = 8000;
        int maxDeg = 260;

        needleAngle = 220 - 260 * (myCar.currentRPM / maxRPM);

        rpmNeedle.transform.eulerAngles = new Vector3(0, 0, needleAngle);  // rpmNeedle.transform.eulerAngles.z + 1

        // spin wheels
        /*GameObject wheelRR = transform.Find("wheels").Find("rearRight").gameObject;
        wheelRR.transform.eulerAngles = new Vector3(transform.eulerAngles.x + needleAngle, transform.eulerAngles.y, transform.eulerAngles.z);

        Debug.Log(wheelRR.transform.eulerAngles.x);*/

        //wheelRR.transform.Rotate(new Vector3(0, 0, 5) * Time.deltaTime * 10);


        // Car movement Translate and Rotate
        transform.Translate(new Vector3(0, 0, myCar.currentSpeed /40) * Time.deltaTime);
        if (myCar.currentSpeed > 0) {
            transform.Rotate(new Vector3(0, horizontal / 30, 0) * Time.deltaTime * 1000);
        }
        else if (myCar.currentSpeed < 0)
        {
            transform.Rotate(new Vector3(0, -horizontal / 30, 0) * Time.deltaTime * 1000);
        }

        //Debug.Log(Time.fixedDeltaTime);
        //Debug.Log(Time.deltaTime);

        //GameObject childObject = transform.Find("v10_italian").gameObject;
        //RealisticEngineSound childScript = childObject.GetComponent<RealisticEngineSound>();


        // Engine Noise
        GameObject childObject = transform.Find("v8_italian").gameObject;
        RealisticEngineSound_mobile childScript = childObject.GetComponent<RealisticEngineSound_mobile>();


        childScript.engineCurrentRPM = myCar.currentRPM;
        if (childScript != null)
        {
            //Debug.Log(childScript.carMaxSpeed);
        }
        else
        {
            Debug.Log("aaaaaaaaaaa");
        }


        // Gear Shifting
        myCar.isManual = true;
        if (Input.GetKeyDown("q"))
        {
            myCar.manualShiftDown();
        }
        else if (Input.GetKeyDown("e"))
        {
            myCar.manualShiftUp();
        }
    }
}


[Serializable]
public class Car
{
    public bool isManual { get; set; }
    //public GearBox gearBox = new GearBox5SpeedBMWe30();
    //public GearBox gearBox = new GearBox6SpeedMcLarenF1();
    // public GearBox gearBox = new GearBox7SpeedMcLaren720s();
    public GearBox gearBox = new GearBox7Speed_FerrariF8();
    public int maxWheelTurnAngle = 40;
    public float rpmConvertCoef = 3f / 50f;
    public float engineRedLine = 8000f;
    public float baseStrenght = 100f;
    public float currentSpeed { get; set; }

    public float speedInKM
    {
        get => (float)Math.Round(currentSpeed / 10, 1);
    }
    public float currentRPM
    {
        get => (gearBox.getGearRatio() != 0) ? (this.speedInKM * gearBox.getGearRatio() * gearBox.finalDriveRatio) / (this.rpmConvertCoef * gearBox.tireCircumInMeter) : (1500f);
    }

    public Car() {}

    public Car(float engineRedLine, int maxWheelTurnAngle)
    {
        this.engineRedLine = engineRedLine;
        this.maxWheelTurnAngle = maxWheelTurnAngle;
    }

    public Car(float engineRedLine, int maxWheelTurnAngle, float currentSpeed, int currentGear)
    {
        this.engineRedLine = engineRedLine;
        this.maxWheelTurnAngle = maxWheelTurnAngle;
        this.currentSpeed = currentSpeed;
        gearBox.currentGear = currentGear;
    }

    public void accelerate(float vertical)
    {
        if (vertical > 0) // accelerate
        {
            if (currentRPM < engineRedLine + 1000)
            {
                currentSpeed += getAccelerationStrenght() * Time.deltaTime;
            }

            if (!isManual)
            {
                if (currentRPM > engineRedLine)
                {
                    gearBox.upShift();
                }
                else if (currentRPM < engineRedLine - 3000f)
                {
                    gearBox.downShift();
                }
            }
        }
        else if (vertical == 0) // idle
        {
            if (currentSpeed < 1 && currentSpeed > -1)
            {
                currentSpeed = 0;
            }
            else if (currentSpeed > 0)
            {
                currentSpeed -= 10 * Time.deltaTime;
            }
            else if (currentSpeed < 0)
            {
                currentSpeed += 10 * Time.deltaTime;
            }
        }
        else if (vertical < 0) // break/reverse
        {
            if (currentSpeed > 0)
            {
                // currentSpeed -= (1 - 2 * vertical); // break
                currentSpeed += vertical * 300 * Time.deltaTime; // break (the vertical is negative here)
            }
            else if (currentSpeed <= 0)
            {
                currentSpeed += vertical * 10 * Time.deltaTime; // reverse
            }
        }
        if ((vertical == 1 || vertical == 0 || vertical == -1) && (currentSpeed > 1 && currentSpeed < -1))
        {
            currentSpeed = Convert.ToInt32(Math.Floor(currentSpeed));
        }

        if (!isManual) {
            //if (currentRPM < 1500f)
            if (currentRPM < 4500f)
            {
                gearBox.downShift();
            }
        }

        if (currentRPM > engineRedLine)
        {
            currentSpeed -= 1000 * Time.deltaTime;
        }

    }

    public void manualShiftUp()
    {
        if (isManual)
        {
            gearBox.upShift();
        }
    }

    public void manualShiftDown()
    {
        if (isManual)
        {
            gearBox.downShift();
        }
    }

    public int getAccelerationStrenght()
    {
        return (int)(baseStrenght * gearBox.getGearRatio());
    }

    public void turnWheels()
    {

        /*angle += horizontal * 40 * Time.deltaTime;
        angle = Mathf.Clamp(transform.localRotation.eulerAngles.y + angle, transform.localRotation.eulerAngles.y - 30, transform.localRotation.eulerAngles.y + 30);
        if (horizontal == 0 && angle != 0f)
        {
            angle += rotateBack * Time.deltaTime;
            if (angle > 0)
            {
                rotateBack = -30f;
            }
            else
            {
                rotateBack = 30f;
            }
        }*/

        //float vertical = Input.GetAxis("Vertical");
        //float horizontal = Input.GetAxis("Horizontal");
        //rigidBody.AddForce(new Vector3(-vertical, 0, 0) * strenght);

        //foreach (var wheel in wheels)
        //{
            //wheel.transform.Rotate(new Vector3(0, (horizontal) / 30, 0) * Time.deltaTime * 1000);
            //wheel.transform.rotation = Quaternion.Euler(0f, angle, 0f);
            //Debug.Log("A");
            //Debug.Log(wheel.transform.rotation);
            //transform.Rotate(new Vector3(0, (vertical * horizontal) / 30, 0) * Time.deltaTime * 1000);
        //}
    }
}


public class GearBox // some default gearbox
{
    public int gearCount = 7;
    public int currentGear = 1;
    public float finalDriveRatio = 2.37f;
    public float tireCircumInMeter = 2f;

    public GearBox(int gearCount, int currentGear, float finalDriveRatio, float tireCircumInMeter)
    {
        this.gearCount = gearCount;
        this.currentGear = currentGear;
        this.finalDriveRatio = finalDriveRatio;
        this.tireCircumInMeter = tireCircumInMeter;
    }

    public void upShift()
    {
        if (currentGear < gearCount)
        {
            currentGear++;
        }
    }

    public void downShift()
    {
        if (currentGear > 1)
        {
            currentGear--;
        }
    }

    public virtual float getGearRatio()
    {
        switch (currentGear)
        {
            case -1:
                return -2.80f;
            case 0: // neutral
                return 0f;
            case 1:
                return 3f;
            case 2:
                return 2f;
            case 3:
                return 1.5f;
            case 4:
                return 1.2f;
            case 5:
                return 1f;
            case 6:
                return 0.85f;
            case 7:
                return 0.70f;
            default:
                return 0f;
        }
    }
}

public class GearBox6SpeedMcLarenF1 : GearBox 
{
    public GearBox6SpeedMcLarenF1() : base(6, 1, 2.37f, 1.787f) {}

    public override float getGearRatio()
    {
        switch (currentGear)
        {
            case -1:
                return -2.80f;
            case 0: // neutral
                return 0f;
            case 1:
                return 3.23f;
            case 2:
                return 2.19f;
            case 3:
                return 1.71f;
            case 4:
                return 1.39f;
            case 5:
                return 1.16f;
            case 6:
                return 0.93f;
            default:
                return 0f;
        }
    }
}

public class GearBox7SpeedMcLaren720s : GearBox
{
    public GearBox7SpeedMcLaren720s() : base(7, 1, 2.37f, 2.3f) {}

    public override float getGearRatio()
    {
        switch (currentGear)
        {
            case -1:
                return -2.80f;
            case 0: // neutral
                return 0f;
            case 1:
                return 3.98f;
            case 2:
                return 2.61f;
            case 3:
                return 1.91f;
            case 4:
                return 1.48f;
            case 5:
                return 1.16f;
            case 6:
                return 0.91f;
            case 7:
                return 0.69f;
            default:
                return 0f;
        }
    }
}

// 1st: 3.72 2nd 2.40 3rd 1.77 4th 1.26 5th 1.00 Reverse 4.23:1
public class GearBox5SpeedBMWe30 : GearBox
{
    public GearBox5SpeedBMWe30() : base(5, 1, 4.23f, 2f) { }

    public override float getGearRatio()
    {
        switch (currentGear)
        {
            case -1:
                return -4.23f;
            case 0: // neutral
                return 0f;
            case 1:
                return 3.72f;
            case 2:
                return 2.40f;
            case 3:
                return 1.77f;
            case 4:
                return 1.26f;
            case 5:
                return 1.0f;
            default:
                return 0f;
        }
    }
}

public class GearBox7Speed_FerrariF8 : GearBox
{
    public GearBox7Speed_FerrariF8() : base(7, 1, 4.375f, 2.1f) { }

    public override float getGearRatio()
    {
        switch (currentGear)
        {
            case -1:
                return -2.979f;
            case 0: // neutral
                return 0f;
            case 1:
                return 3.334f;
            case 2:
                return 2.285f;
            case 3:
                return 1.728f;
            case 4:
                return 1.369f;
            case 5:
                return 1.115f;
            case 6:
                return 0.875f;
            case 7:
                return 0.642f;
            default:
                return 0f;
        }
    }
}