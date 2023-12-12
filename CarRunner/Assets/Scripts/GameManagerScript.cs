using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour
{
    public GameObject[] npcCars;
    public int roadLength = 40;
    private GameObject player;
    private GameObject highwaySpawner;
    private List<GameObject> cars = new List<GameObject>();

    public float spawnIntervalMin = 1f;
    public float spawnIntervalMax = 4f;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        highwaySpawner = GameObject.FindGameObjectWithTag("MainRoadSpawner");
        for (int i = 0; i < 4; i++)
        {
            SpawnObject();
        }
        InvokeRepeating("SpawnObject", Random.Range(spawnIntervalMin, spawnIntervalMax), Random.Range(spawnIntervalMin, spawnIntervalMax));
    }

    // Update is called once per frame
    void Update()
    {
    }

    GameObject GetRandomCar()
    {
        return npcCars[Random.Range(0, npcCars.Length)];
    }
    void SpawnObject()
    {
        ProceduralHighwayScript highway = highwaySpawner.GetComponent<ProceduralHighwayScript>();
        Vector3 position = new Vector3(player.transform.position.x + Random.Range(roadLength*0.2f, roadLength*2), 0.1f, highway.lanes[Random.Range(0, highway.lanes.Count - 1)].transform.position.z);

        GameObject spawnedNPC = Instantiate(GetRandomCar(), position, Quaternion.Euler(0f, 90f, 0f));
        cars.Add(spawnedNPC);

        Rigidbody npcRigidbody = spawnedNPC.GetComponent<Rigidbody>();
        if (npcRigidbody != null)
        {
            float speed = 10f;
            npcRigidbody.velocity = spawnedNPC.transform.forward * speed;
        }
        spawnedNPC.AddComponent<DestroyCasesNPCCars>();
    }
}
