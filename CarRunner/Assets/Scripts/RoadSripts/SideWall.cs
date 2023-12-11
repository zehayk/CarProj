using System;
using Unity.VisualScripting;
using UnityEngine;

public class SideWall : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public Boolean isLeftSide;
    public Material wallMaterial;
    public GameObject CubePrefab;
    private GameObject Cube;
    public float HighwayHeight = 4.88f;
    public int roadLength = 40;

    private void Start()
    {
        GenerateWall();
    }

    private void GenerateWall()
    {
        float zDisplacement = isLeftSide ? 0.5f : -0.5f;
        Vector3 absolutePosition = Vector3.Lerp(startPoint, endPoint, 0.5f);
        float distance = Vector3.Distance(startPoint, endPoint); 
        Cube = Instantiate(CubePrefab, new Vector3(absolutePosition.x, absolutePosition.y+HighwayHeight/ 2, absolutePosition.z + zDisplacement), Quaternion.identity);
        Cube.transform.localScale = new Vector3(distance, HighwayHeight, 1);
    }
    private Vector3 GetEndPoint(Vector3 firstPoint)
    {
        return new Vector3(firstPoint.x+ roadLength, firstPoint.y, firstPoint.z);
    }
    private void OnDestroy()
    {
        Destroy(Cube);
    }
}
