using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    MeshRenderer wallMesh;
    private void Start()
    {
        wallMesh = GetComponent<MeshRenderer>();
        wallMesh.material.color = GameManager.Instance.WallColor;

        this.transform.localScale = Vector3.zero;
    }
}
