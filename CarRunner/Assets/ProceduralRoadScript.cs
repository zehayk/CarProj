using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralRoadScript : MonoBehaviour
{
    public int roadLength = 20;
    public GameObject BaseRoad;
    private Queue<GameObject> RoadQueue = new Queue<GameObject>();
    private GameObject player;
    private GameObject currentRoad;
    private Vector3 LastPosition;
    private Vector3 FirstPosition;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentRoad = Instantiate(BaseRoad, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        CurvedRoadPiece myScriptComponent = currentRoad.GetComponent<CurvedRoadPiece>();
        myScriptComponent.startPoint = this.transform.position;
        myScriptComponent.endPoint = GetRandomPoint(myScriptComponent.startPoint);
        LastPosition = myScriptComponent.endPoint;
        FirstPosition = myScriptComponent.startPoint;
        RoadQueue.Enqueue(currentRoad);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, LastPosition) < roadLength*4f)
        {
            AddEndSection();
        }
        if (Vector3.Distance(player.transform.position, FirstPosition) >= roadLength * 3f)
        {

            RemoveFirstRoadSection();
        }
    }
    private void AddEndSection()
    {
        Vector3 PrevEndPoint = LastPosition;
        Vector3 NewEndPoint = GetRandomPoint(PrevEndPoint);

        currentRoad = Instantiate(BaseRoad, new Vector3(0, 0, 0), Quaternion.identity);

        CurvedRoadPiece CurrendRoadSettings = currentRoad.GetComponent<CurvedRoadPiece>();
        CurrendRoadSettings.startPoint = PrevEndPoint;
        CurrendRoadSettings.endPoint = NewEndPoint;

        RoadQueue.Enqueue(currentRoad);

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
        float NumZ = Random.Range(-5, 5);
        Vector3 lastPointAdd = new Vector3(lastVector.x + 20, 0, lastVector.z + NumZ);
        return lastPointAdd;
    }
}
