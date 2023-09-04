using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;
using TMPro;


//2815692_Mclaren+Senna
public class move : MonoBehaviour
{
    public TextMeshProUGUI speedoMeter;
    GameObject[] wheels;
    Quaternion startRotation;
    float angle = 0f;
    float rotateBack = 0f;
    //float rotSpeed = 50;


    float strenght = 50.0f;
    Rigidbody rigidBody;

    public float rpmConvertCoef = 3f / 50f;
    public Car myCar;


    // Start is called before the first frame update
    void Start()
    {
        myCar = new Car(140, 7000, 40);
        //myCar = new Car(140, 7000, 40, 1550, 0, 2);
        wheels = GameObject.FindGameObjectsWithTag("FrontWheels");
        //foreach (var wheel in wheels) { 
        //    wheel.transform.rotation = startRotation;
        //}
        rigidBody = GetComponent<Rigidbody>();


        GameObject tmpGO = GameObject.Find("SpeedoMeter");
        speedoMeter = tmpGO.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        float vertical = 0f;// Input.GetAxis("Vertical"); // forward backward
        float horizontal = 0f;// Input.GetAxis("Horizontal");

        vertical = Input.GetAxis("Jump");

        Debug.Log(vertical);

        /*if (vertical > 0) // accelerate
        {
            myCar.accelerate(true);
        }
        else if (vertical == 0) // idle throttle
        {
            myCar.accelerate(false);
        }
        else if (vertical < 0) // reverse/breaking
        {
            myCar.accelerate(false, vertical);
        }*/

        myCar.accelerate(vertical);


        //Debug.Log(horizontal);
        //Debug.Log(vertical);

        //Debug.Log(myCar.currentSpeed);


        //float a = Math.Floor(myCar.currentRPM / 10);
        float speedInKM = (float)Math.Round(myCar.currentSpeed / 10, 1);
        //float ratio = 2.19f;
        float ratio = myCar.getGearRatio();
        // double a = Math.Round(((((myCar.currentSpeed / 60) * ratio) / myCar.tireCircumInMeter)) / myCar.finalDriveRatio, 2);


        float engineRPM = (speedInKM * ratio * myCar.finalDriveRatio) / (rpmConvertCoef * myCar.tireCircumInMeter);

        speedoMeter.SetText("G" + myCar.currentGear + "\n" + engineRPM + " RPM\n" + speedInKM + "km/h\n" /* + myCar.currentSpeed*/);

        if (engineRPM > myCar.engineRedLine)
        {
            myCar.upShift();
        }


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

    }
}


public class Car
{
    //int gearCount;
    float torque;
    float maxRPM;
    int maxWheelTurnAngle;
    public float currentSpeed { get; set; }
    public float currentRPM = 0f;
    public int currentGear = 1;

    public float engineRedLine = 7500f;
    public float baseStrenght = 300f;
    public float tireCircumInMeter = 1.787f; // 2.3f;
    public float finalDriveRatio = 2.37f;


    private bool _clutch = false;
    public bool Clutch
    {
        get { return _clutch; }
        set { _clutch = currentSpeed == 0; }
    }


    public Car(float torque, float maxRPM, int maxWheelTurnAngle)
    {
        this.torque = torque;
        this.maxRPM = maxRPM;
        this.maxWheelTurnAngle = maxWheelTurnAngle;
    }

    public Car(float torque, float maxRPM, int maxWheelTurnAngle, float currentSpeed, float currentRPM, int currentGear)
    {
        this.torque = torque;
        this.maxRPM = maxRPM;
        this.maxWheelTurnAngle = maxWheelTurnAngle;
        this.currentSpeed = currentSpeed;
        this.currentRPM = currentRPM;
        this.currentGear = currentGear;
    }

    public void carMovement()
    {
        //accelerate();


        turnWheels();
    }

    public void accelerate(float vertical)
    {
        if (vertical > 0) // accelerate
        {
            currentSpeed += getAccelerationStrenght() * Time.deltaTime;
        }
        /*else if (vertical == 0) // idle
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
        }*/
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
        /*if ((vertical == 1 || vertical == 0 || vertical == -1) && (currentSpeed > 1 && currentSpeed < -1))
        {
            currentSpeed = Convert.ToInt32(Math.Floor(currentSpeed));
        }*/
    }
    
    public int getAccelerationStrenght()
    {
        return (int)(baseStrenght / getGearRatio());
    }

    public float getGearRatio()
    {
        switch (currentGear)
        {
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
        /*switch (currentGear)
        {
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
        }*/
    }

    public void upShift()
    {
        if (currentGear < 7)
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