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
        PathCreator startRoad = Instantiate(BaseRoad, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z), Quaternion.identity);
        BaseRoad.bezierPath.GlobalNormalsAngle = 0f;
        BaseRoad.bezierPath.ControlPointMode = BezierPath.ControlMode.Automatic;
        BaseRoad.bezierPath.GetPoint(0).Set(-20,0,0);
        BaseRoad.TriggerPathUpdate();
    }

    // Update is called once per frame


    void Update()
    {
        if (Vector3.Distance(player.transform.position, BaseRoad.bezierPath.GetPoint(BaseRoad.bezierPath.NumAnchorPoints - 1)) < 20f) {
            AddEndSection();
            BaseRoad.TriggerPathUpdate();
        }
        if (Vector3.Distance(player.transform.position, BaseRoad.path.GetPoint(0)) >= 30f)
        {
            
            // RemoveFirstRoadSection();
        }
    }

    private void AddEndSection()
    {
        // PathCreator roadSection = Instantiate(GetRandomSection(), lastRoadTransform, Quaternion.identity);
        // Vector3 RealPositionRoad = (lastPoint - StartingPoint);
        
            Debug.Log(Vector3.Distance(player.transform.position, BaseRoad.bezierPath.GetPoint(BaseRoad.bezierPath.NumAnchorPoints - 1)));
            Vector3 newPointInPath = GetRandomPoint(BaseRoad.bezierPath.GetPoint(BaseRoad.bezierPath.NumAnchorPoints - 1));
            BaseRoad.bezierPath.AddSegmentToEnd(newPointInPath);
        
        
    }
    private void RemoveFirstRoadSection()
    {
    }
    private Vector3 GetRandomPoint(Vector3 lastPointX)
    {
        float NumX = Random.Range(0, 20);
        float NumZ = Random.Range(-5, 5);
        // Use lastPointX as an offset for generating a new random point
        Vector3 lastPointAdd = new Vector3(lastPointX.x + NumX + 20, 0, lastPointX.z + NumZ);
        return lastPointAdd;
    }
}
