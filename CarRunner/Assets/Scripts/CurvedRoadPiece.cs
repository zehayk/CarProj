using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(MeshCollider))]
public class CurvedRoadPiece : MonoBehaviour
{
    public Vector3 startPoint;
    public Vector3 endPoint;
    public float roadWidth = 4f;
    public int segments = 10;
    public Material roadMaterial;
    private GameObject previousRoad;

    private void Start()
    {
        startPoint += this.transform.position;
        endPoint += this.transform.position;
        GenerateRoad();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer.material != null)
        {
            // Optionally, adjust other MeshRenderer properties, e.g., materials
            meshRenderer.material = roadMaterial;
        }
    }

    public void Initialize(GameObject prevroad)
    {
        previousRoad = prevroad;

    }

    private void GenerateRoad()
    {
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();

        Mesh mesh = meshFilter.mesh;
        mesh.Clear();

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uv = new List<Vector2>();

        // Moving start based on previous location
        /*Vector3 currentRoadDirection = (endPoint - startPoint);
        Vector3 previousRoadDirection = (previousRoad.GetComponent<CurvedRoadPiece>().endPoint - previousRoad.GetComponent<CurvedRoadPiece>().startPoint);

        float angleChange = Vector3.Angle(previousRoadDirection, currentRoadDirection);

        float x = roadWidth * Mathf.Sin(angleChange) * Mathf.Cos(angleChange);
        float z = roadWidth * Mathf.Pow(Mathf.Sin(angleChange), 2);*/

        //startPoint = startPoint + new Vector3(x, startPoint.y, z);

        // Debug.Log("Start Point:" + startPoint);

        // Triangle Mesh Creation
        for (int i = 0; i <= segments; i++)
        {
            float t = i / (float)segments;
            Vector3 pointOnLine = Vector3.Lerp(startPoint, endPoint, t);

            // Generate road vertices
            Vector3 rightOffset = Vector3.Cross((endPoint - startPoint).normalized, Vector3.down).normalized * roadWidth * 0.5f;

            if (t == 0 || t == 1)
            {
                rightOffset = new Vector3(rightOffset.x, rightOffset.y, rightOffset.z);
            }
            else
            {
                rightOffset = new Vector3(rightOffset.x, rightOffset.y, rightOffset.z);
            }

            vertices.Add(pointOnLine + rightOffset);
            vertices.Add(pointOnLine - rightOffset);

            // Generate road triangles
            if (i < segments)
            {
                int baseIndex = i * 2;
                triangles.Add(baseIndex);
                triangles.Add(baseIndex + 1);
                triangles.Add(baseIndex + 2);

                triangles.Add(baseIndex + 2);
                triangles.Add(baseIndex + 1);
                triangles.Add(baseIndex + 3);
            }

            // Generate UV coordinates for the main mesh
            float u = i / (float)segments;
            uv.Add(new Vector2(u, 0f));
            uv.Add(new Vector2(u, 1f));
        }

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uv.ToArray();
        mesh.RecalculateNormals();

        // Assign the mesh to the MeshCollider
        meshCollider.sharedMesh = mesh;
    }
}