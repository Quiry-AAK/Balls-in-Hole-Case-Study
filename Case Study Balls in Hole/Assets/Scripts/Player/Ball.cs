using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    [SerializeField] Transform hole;
    [SerializeField] ParticleSystem distortionFx;
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

        else if(other.CompareTag("Trap"))
        {
            ActiveDistortionFx(Color.black, Color.blue, transform.position);

            GameManager.Instance.FinishGame(false);
            Destroy(gameObject, 0.05f);
        }

    }

    void ActiveDistortionFx(Color minGradient, Color maxGradient, Vector3 where)
    {
        distortionFx.gameObject.SetActive(true);
            distortionFx.transform.position = where;
            ParticleSystem.MainModule main = distortionFx.main;
            main.startColor = new ParticleSystem.MinMaxGradient(minGradient, maxGradient);
            distortionFx.Play();
            Invoke("CloseFx", 0.5f);
    }

    void CloseFx()
    {
        distortionFx.gameObject.SetActive(false);
    }

    public void CollideGlass(GameObject glass, Vector3 fxPos)
    {
        GetComponent<PlayerMovement>().Direction = Vector3.zero;
        ActiveDistortionFx(Color.white, Color.gray, fxPos);
        Destroy(glass.gameObject, 0.05f);
    }

}
