using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public GameObject controlsMenu;
    
    // Start is called before the first frame update
    void Start()
    {
        controlsMenu = GameObject.FindGameObjectWithTag("ControlsMenu");
        controlsMenu.SetActive(false);
    }

}
