using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCCarsBehavior : MonoBehaviour
{
    private int roadLength = 40;
    private GameObject player;
    private Vector3 EndPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        EndPosition = new Vector3(transform.position.x + 1000, transform.position.y, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.LookRotation(EndPosition);
        if (Vector3.Distance(player.transform.position, transform.position) >= roadLength * 14f)
        {
            Destroy(gameObject);
        }
    }
}
