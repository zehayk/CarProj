using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject[] npcCars;
    private GameObject player;
    private GameObject highwaySpawner;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        highwaySpawner = GameObject.FindGameObjectWithTag("MainRoadSpawner");
    }

    // Update is called once per frame
    void Update()
    {
        ProceduralHighwayScript highway = highwaySpawner.GetComponent<ProceduralHighwayScript>();

    }

    void RandomSpawn()
    {
        GameObject spawnedNPC = Instantiate(objectToSpawn, transform.position, Quaternion.identity);
    }
    GameObject GetRandomCar()
    {
        float num = Random.Range(0, npcCars);
        
    }
}
