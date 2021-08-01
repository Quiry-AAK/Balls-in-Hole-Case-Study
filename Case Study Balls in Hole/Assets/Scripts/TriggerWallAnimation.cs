using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerWallAnimation : MonoBehaviour
{
    [Range (10,11)] public int WhichLayer;

    Vector3 endScale;

    private void OnEnable() 
    {
        transform.position = new Vector3(0, 0.45f, -10);
        
        transform.DOLocalMoveZ(10, 1f).OnComplete(SetPassive).SetDelay(.5f); // SetDelay is for my unity hub. Because my editor crashes on the starting of playing game. That's why I delayed.
    }

    void SetPassive()
    {
        gameObject.SetActive(false);
        GameManager.Instance.IsGameReadyToStart = true;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.layer == WhichLayer)
        {
            if(WhichLayer == 12)
                endScale = new Vector3(1,10,1);
            else
                endScale =  Vector3.one;

            other.transform.DOScale(endScale, 1f);
        }
    }


}
