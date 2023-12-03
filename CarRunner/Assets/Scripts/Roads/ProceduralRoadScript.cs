using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;
using Unity.VisualScripting;

public class ProceduralRoadScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 StartingPoint;
    public PathCreator Left90;
    public PathCreator Right90;
    public PathCreator Straight;
    private GameObject player;
    private List<PathCreator> roadSectionList = new List<PathCreator>();
    private int rotation; // 0 = Nord; 1=Est; 2=Sud; 3=Ouest; 
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PathCreator startRoad = Instantiate(Straight, new Vector3(player.transform.position.x, 0, player.transform.position.z), Quaternion.identity);
        roadSectionList.Add(startRoad);

    }

    // Update is called once per frame


    void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, roadSectionList.Last().transform.position) <= 40) {
            if (roadSectionList.Count > 5) {
                RemoveFirstRoadSection();
            }
            AddEndSection();
        }
    }

    private void AddEndSection()
    {
        Vector3 lastRoadTransform = roadSectionList.Last().path.GetPoint(1);
        PathCreator roadSection = Instantiate(GetRandomSection(), lastRoadTransform, Quaternion.identity);
        StartingPoint = roadSection.path.GetPoint(0);
        Vector3 RealPositionRoad = roadSection.transform.position + (lastRoadTransform - StartingPoint);
        roadSection.transform.position = RealPositionRoad;
        roadSectionList.Add(roadSection);

    }
    private void RemoveFirstRoadSection()
    {
        PathCreator lastRoad = roadSectionList.First();
        roadSectionList.RemoveAt(0);
        Destroy(lastRoad);
    }     
    private PathCreator GetRandomSection()
    {
        PathCreator[] objects = { Left90, Right90, Straight };
        
        return objects[Random.Range(0, objects.Length)];
    }
}
