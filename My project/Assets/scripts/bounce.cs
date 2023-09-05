using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bounce : MonoBehaviour
{
    float timer = 0;
    Vector3 movement = new Vector3(0, 1, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if ( timer > 1.0f )
        {
            timer = 0f;
            movement = -1 * movement;
        }
        transform.Translate(movement * Time.deltaTime);
    }
}
