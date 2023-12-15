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

    public float spawnIntervalMin = 0.5f;
    public float spawnIntervalMax = 3f;

    void Start()
    {
        // Find all GameObjects with the specified tags
        GameObject[] objectsWithTag1 = GameObject.FindGameObjectsWithTag("EnemyCop");
        GameObject[] objectsWithTag2 = GameObject.FindGameObjectsWithTag("Enemy");

        // Iterate through all combinations and disable collisions
        foreach (GameObject obj1 in objectsWithTag1)
        {
            foreach (GameObject obj2 in objectsWithTag2)
            {
                Collider collider1 = obj1.GetComponent<Collider>();
                Collider collider2 = obj2.GetComponent<Collider>();

                if (collider1 != null && collider2 != null)
                {
                    Physics.IgnoreCollision(collider1, collider2);
                }
            }
        }


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
        if (highway.lanes.Count > 0)
        {
            Vector3 position = new Vector3(player.transform.position.x + Random.Range(roadLength * 8f, roadLength * 10f), 0.1f, highway.lanes[Random.Range(0, highway.lanes.Count)].transform.position.z);

            GameObject spawnedNPC = Instantiate(GetRandomCar(), position, Quaternion.Euler(0f, 90f, 0f));
            spawnedNPC.AddComponent<NPCCarsBehavior>();
            cars.Add(spawnedNPC);

            spawnedNPC.tag = "Enemy";
            SetTagRecursively(spawnedNPC.transform, "Enemy");

            Rigidbody npcRigidbody = spawnedNPC.GetComponent<Rigidbody>();
            if (npcRigidbody != null)
            {
                float speed = 20f;
                npcRigidbody.velocity = spawnedNPC.transform.forward * speed;
            }
        }
        
    }
    void SetTagRecursively(Transform parent, string tag)
    {
        foreach (Transform child in parent)
        {
            // Set the tag for the child object
            child.gameObject.tag = tag;

            // Recursively set the tag for the child's children
            SetTagRecursively(child, tag);
        }
    }
}
