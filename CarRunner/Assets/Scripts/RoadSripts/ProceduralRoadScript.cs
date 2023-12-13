using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRoadScript : MonoBehaviour
{
    public int roadLength = 40;
    public GameObject BaseRoad;
    private Queue<GameObject> RoadQueue = new Queue<GameObject>();
    private GameObject player;
    private GameObject currentRoad;
    public Vector3 StartingPoint;
    private Vector3 LastPosition;
    private Vector3 FirstPosition;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (StartingPoint == null)
        {
            StartingPoint = new Vector3(0, 0, 0);
        }
        currentRoad = Instantiate(BaseRoad, new Vector3(StartingPoint.x, 0, StartingPoint.z), Quaternion.identity);
        CurvedRoadPiece myScriptComponent = currentRoad.GetComponent<CurvedRoadPiece>();
        myScriptComponent.startPoint = StartingPoint;
        myScriptComponent.endPoint = GetRandomPoint(myScriptComponent.startPoint);
        LastPosition = myScriptComponent.endPoint;
        FirstPosition = myScriptComponent.startPoint;
        myScriptComponent.Initialize(currentRoad);
        RoadQueue.Enqueue(currentRoad);
    }
    
    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, LastPosition) < roadLength*15f)
        {
            AddEndSection();
        }
        if (Vector3.Distance(player.transform.position, FirstPosition) >= roadLength * 15f)
        {

            RemoveFirstRoadSection();
        }
    }
    private void AddEndSection()
    {
        Vector3 PrevEndPoint = LastPosition;
        Vector3 NewEndPoint = GetRandomPoint(PrevEndPoint);

        GameObject NextRoad = Instantiate(BaseRoad, new Vector3(0, 0, 0), Quaternion.identity);

        CurvedRoadPiece CurrendRoadSettings = NextRoad.GetComponent<CurvedRoadPiece>();
        CurrendRoadSettings.startPoint = PrevEndPoint;
        CurrendRoadSettings.endPoint = NewEndPoint;
        CurrendRoadSettings.Initialize(currentRoad);
        RoadQueue.Enqueue(NextRoad);
        currentRoad = NextRoad;

        // Update LastPosition after setting the endPoint
        LastPosition = CurrendRoadSettings.endPoint;
    }
    private void RemoveFirstRoadSection()
    {
        GameObject variableName = RoadQueue.Peek();
        FirstPosition = variableName.GetComponent<CurvedRoadPiece>().endPoint;
        Destroy(variableName);
        RoadQueue.Dequeue();
    }

    private Vector3 GetRandomPoint(Vector3 lastVector)
    {
        // float NumZ = Random.Range(-15, 15);
        float NumZ = 0;
        Vector3 lastPointAdd = new Vector3(lastVector.x + roadLength, lastVector.y, lastVector.z + NumZ);
        return lastPointAdd;
    }
}
