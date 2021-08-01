using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    [SerializeField] Transform hole;
    [SerializeField] GameObject ballGraphic;
    public bool isCaptured = false;
    

    private void FixedUpdate() 
    {
        if(isCaptured)
        {
            this.transform.position = new Vector3(hole.transform.position.x, -0.7f, hole.transform.position.z);
        }

    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Hole"))
        {
            isCaptured = true;

            PlayerMovement playerMovement = GetComponent<PlayerMovement>();
            Destroy(playerMovement);

            GameManager.Instance.BallAndHoleCollided();

            return;
        }

    }

}
