using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public GameObject CarCam;
    public GameObject RearViewCam;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            RearViewCam.SetActive(true);
            CarCam.SetActive(false);
        }
        else
        {
            CarCam.SetActive(true);
            RearViewCam.SetActive(false);
        }
    }
}
