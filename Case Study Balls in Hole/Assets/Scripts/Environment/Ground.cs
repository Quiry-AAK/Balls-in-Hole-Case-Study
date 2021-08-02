using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    MeshRenderer groundMesh;

    private void Start() {
        groundMesh = GetComponent<MeshRenderer>();
        groundMesh.material.color = GameManager.Instance.GroundColor;
    }
}
