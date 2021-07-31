using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TriggerWallAnimation : MonoBehaviour
{
    public string WallTag;


    private void OnEnable() 
    {
        transform.position = new Vector3(0,2, -10);
        
        transform.DOLocalMoveZ(10, 1f).OnComplete(SetPassive).SetDelay(.5f); // SetDelay is for my unity hub. Because my editor crashes on the starting of playing game. That's why I delayed.
    }

    void SetPassive()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.CompareTag(WallTag))
        {
            other.transform.DOScale(1f, 1f);
        }
    }

}
