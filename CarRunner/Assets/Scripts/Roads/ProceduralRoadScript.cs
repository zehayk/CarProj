using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
using System.Linq;
using Unity.VisualScripting;
using System.IO;

public class ProceduralRoadScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Vector3 FirstPoint;
    public PathCreator BaseRoad;
    private GameObject player;
    private int rotation; // 0 = Nord; 1=Est; 2=Sud; 3=Ouest; 
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PathCreator startRoad = Instantiate(BaseRoad, new Vector3(player.transform.position.x, 0, player.transform.position.z), Quaternion.identity);
        BaseRoad.bezierPath.GlobalNormalsAngle = 90f;
        BaseRoad.TriggerPathUpdate();
    }

    // Update is called once per frame


    void FixedUpdate()
    {
        if (Vector3.Distance(player.transform.position, BaseRoad.path.GetPoint(1)) < 20f) {
            AddEndSection();
        }
        if (Vector3.Distance(player.transform.position, BaseRoad.path.GetPoint(0)) >= 30f)
        {
            RemoveFirstRoadSection();
        }
    }

    private void AddEndSection()
    {
        // PathCreator roadSection = Instantiate(GetRandomSection(), lastRoadTransform, Quaternion.identity);
        // Vector3 RealPositionRoad = (lastPoint - StartingPoint);
        BaseRoad.bezierPath.AddSegmentToEnd(GetRandomPoint(BaseRoad.path.GetPoint(1)));
        BaseRoad.TriggerPathUpdate();
    }
    private void RemoveFirstRoadSection()
    {
    }
    private Vector3 GetRandomPoint(Vector3 lastPointX)
    {
        float NumX = Random.Range(0, 20);
        float NumZ = 40 - NumX;
        // Use lastPointX as an offset for generating a new random point
        Vector3 lastPointAdd = new Vector3(lastPointX.x + NumX, lastPointX.y, lastPointX.z + NumZ);
        Debug.Log(lastPointAdd);
        return lastPointAdd;
    }
}
