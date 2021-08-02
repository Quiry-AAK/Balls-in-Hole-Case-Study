using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleAndBallCollide : MonoBehaviour
{
    [SerializeField] GameObject confettiFx;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            confettiFx.SetActive(true);
            confettiFx.GetComponentInChildren<ParticleSystem>().Play();
            confettiFx.transform.position = this.transform.position;
            Invoke("DoPassiveParticle", 2f);
        }
        
    }

    void DoPassiveParticle()
    {
        confettiFx.SetActive(false);
    }
}
