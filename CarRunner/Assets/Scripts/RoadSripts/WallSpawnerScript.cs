using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSpawnerScript : MonoBehaviour
{
    public int roadLength = 40;
    public GameObject BaseWall;
    private Queue<GameObject> WallQueue = new Queue<GameObject>();
    private GameObject player;
    private GameObject currentWall;
    public Boolean isLeftSide;
    public Vector3 StartingPoint;
    private Vector3 LastPosition;
    private Vector3 FirstPosition;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (StartingPoint == null)
        {
            StartingPoint = new Vector3(0, 0, 0);
        }
        currentWall = Instantiate(BaseWall, new Vector3(StartingPoint.x, 0, StartingPoint.z), Quaternion.identity);
        SideWall myScriptComponent = currentWall.GetComponent<SideWall>();
        myScriptComponent.startPoint = StartingPoint;
        myScriptComponent.endPoint = GetRandomPoint(myScriptComponent.startPoint);
        LastPosition = myScriptComponent.endPoint;
        FirstPosition = myScriptComponent.startPoint;
        myScriptComponent.isLeftSide = isLeftSide;
        WallQueue.Enqueue(currentWall);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, LastPosition) < roadLength * 12f)
        {
            AddEndSection();
        }
        if (Vector3.Distance(player.transform.position, FirstPosition) >= roadLength * 12f)
        {

            RemoveFirstRoadSection();
        }
    }
    private void AddEndSection()
    {
        Vector3 PrevEndPoint = LastPosition;
        Vector3 NewEndPoint = GetRandomPoint(PrevEndPoint);

        GameObject NextWall = Instantiate(BaseWall, new Vector3(0,0,0), Quaternion.identity);

        SideWall CurrendRoadSettings = NextWall.GetComponent<SideWall>();
        CurrendRoadSettings.startPoint = PrevEndPoint;
        CurrendRoadSettings.endPoint = NewEndPoint;
        CurrendRoadSettings.isLeftSide = isLeftSide;
        WallQueue.Enqueue(NextWall);
        currentWall = NextWall;

        // Update LastPosition after setting the endPoint
        LastPosition = CurrendRoadSettings.endPoint;
    }
    private void RemoveFirstRoadSection()
    {
        GameObject wall = WallQueue.Peek();
        FirstPosition = wall.GetComponent<SideWall>().endPoint;
        Destroy(wall);
        WallQueue.Dequeue();
    }

    private Vector3 GetRandomPoint(Vector3 lastVector)
    {
        // float NumZ = Random.Range(-15, 15);
        float NumZ = 0;
        Vector3 lastPointAdd = new Vector3(lastVector.x + roadLength, lastVector.y, lastVector.z + NumZ);
        return lastPointAdd;
    }
}
