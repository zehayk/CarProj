using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ProceduralHighwayScript : MonoBehaviour
{
    public GameObject RoadPrefab;
    private GameObject player;
    public GameObject WallPrefab;
    private List<GameObject> lanes = new List<GameObject>();
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        // Adding lanes
        GameObject ProceduralHighway = null;
        for (int i = 0; i < 4; i++)
        {
            ProceduralHighway = Instantiate(RoadPrefab, new Vector3(this.transform.position.x-120, 0, this.transform.position.z + (4*i - 6)), Quaternion.identity);
            ProceduralRoadScript ProceduralRoadScript = ProceduralHighway.GetComponent<ProceduralRoadScript>();
            ProceduralRoadScript.StartingPoint = ProceduralHighway.transform.position;
            ProceduralHighway.transform.parent = this.transform;
            lanes.Add( ProceduralHighway );
        }

        // Adding walls
        // Left Wall
        ProceduralHighway = Instantiate(WallPrefab, new Vector3(this.transform.position.x - 80, 0, this.transform.position.z + 8), Quaternion.identity);
        WallSpawnerScript WallSpawnerScriptLeft = ProceduralHighway.GetComponent<WallSpawnerScript>();
        WallSpawnerScriptLeft.StartingPoint = ProceduralHighway.transform.position;
        WallSpawnerScriptLeft.isLeftSide = true;
        ProceduralHighway.transform.parent = this.transform;
        // Right Wall
        ProceduralHighway = Instantiate(WallPrefab, new Vector3(this.transform.position.x - 80, 0, this.transform.position.z - 8), Quaternion.identity);
        WallSpawnerScript WallSpawnerScriptRight = ProceduralHighway.GetComponent<WallSpawnerScript>();
        WallSpawnerScriptRight.StartingPoint = ProceduralHighway.transform.position;
        WallSpawnerScriptRight.isLeftSide = false;
        ProceduralHighway.transform.parent = this.transform;
    }

    void Update()
    {
        
    }
}
