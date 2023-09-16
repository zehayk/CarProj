using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineCamFreeLook : MonoBehaviour
{
    private CinemachineFreeLook cinemachineFreeLook;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Camera.main.gameObject.TryGetComponent<CinemachineBrain>(out var brain);
        if (brain == null)
        {
            Debug.Log("nn");
            return;
        }

        cinemachineFreeLook = gameObject.GetComponent<CinemachineFreeLook>();

        float horizontalInput = Input.GetAxis("Horizontal GamepadR");
        float verticalInput = Input.GetAxis("Vertical GamepadR");

        float angle = 0;
        //angle = Math.Atan(horizontalInput, verticalInput);
        angle = Mathf.Atan2(horizontalInput, verticalInput) * Mathf.Rad2Deg;

        Debug.Log("(" + horizontalInput + ", " + verticalInput + ") : " + angle);

        cinemachineFreeLook.m_XAxis.Value = angle;

        //cinemachineFreeLook.m_XAxis.Value += 5;
        Debug.Log(cinemachineFreeLook.m_XAxis.Value);

    }
}
