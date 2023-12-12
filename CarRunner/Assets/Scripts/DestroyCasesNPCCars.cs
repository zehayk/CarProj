using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyCasesNPCCars : MonoBehaviour
{
    private int roadLength = 40;
    private GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) >= roadLength * 10f)
        {
            Destroy(this);
        }
    }
}
