using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TakeCoin : MonoBehaviour
{
    [SerializeField] GameObject takeCoinFX;
    private void OnTriggerEnter(Collider other) 
    {
        if(other.CompareTag("Coin"))
        {
             GameManager.Instance.TakenCoins++;

            // takeCoinFX.SetActive(true);
            // takeCoinFX.transform.localScale = Vector3.one * 0.17f;
            // takeCoinFX.transform.position = other.transform.position;
            // takeCoinFX.transform.DOScale(0.25f, 0.3f).SetEase(Ease.InOutBack).OnComplete(CloseFX);
            
            // Destroy(other.transform.parent.gameObject);
            GameManager.Instance.CoinFxAndDestroy(other.gameObject);
        }

    }

    void CloseFX()
    {
        takeCoinFX.SetActive(false);
    }
}
