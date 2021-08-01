using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchHoleCollider : MonoBehaviour
{
    [SerializeField] PolygonCollider2D hole2DCollider;
    [SerializeField] PolygonCollider2D ground2DCollider;
    public MeshCollider GeneratedMeshCollider;

    Mesh generatedMesh;

    private void FixedUpdate() 
    {
        if(transform.hasChanged)
        {
            transform.hasChanged = false;
            hole2DCollider.transform.position = new Vector2(transform.position.x, transform.position.z);
            MakeHole2D();
        }
    }

    void MakeHole2D()
    {
        Vector2[] pointPositions = hole2DCollider.GetPath(0);

        for (int i = 0; i < pointPositions.Length; i++)
        {
            pointPositions[i] += (Vector2)hole2DCollider.transform.position;
        }

        ground2DCollider.pathCount = 2;
        ground2DCollider.SetPath(1, pointPositions);
    }
    
    void GenerateMeshCollider()
    {
        if(generatedMesh != null)
            Destroy(generatedMesh);
        generatedMesh = ground2DCollider.CreateMesh(true, true);
        GeneratedMeshCollider.sharedMesh = generatedMesh;
    }
}
