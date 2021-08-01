using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    // Singleton
    [HideInInspector] public static GameManager Instance; 

    [SerializeField] GameObject beforeStartingUI;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject triggerWall;

    public Color WallColor;
    public Color GroundColor;

    public Vector3 TriggerWallPos;

    public bool IsGameReadyToStart = false;
    public int TakenCoins;
    public int TotalCoins;


    GameObject[] coins;

    Ball[] balls;
    int capturedBalls;

    private void Awake() 
    {
        Instance = this;
    }

    private void Start() 
    {
        TakenCoins = 0;
        TotalCoins = 0;
        capturedBalls = 0;
        coins = GameObject.FindGameObjectsWithTag("Coin");
        balls = FindObjectsOfType<Ball>();

        if(coins.Length > 0)
            TotalCoins = coins.Length;

        StartSettings();
    }



    public void BallAndHoleCollided()
    {
        capturedBalls++;

        if(capturedBalls >= balls.Length)
        {
            FinishGame();
        }
    }

    void FinishGame()
    {
        UIManager.Instance.FinishGameSettingsForUI(TotalCoins, TakenCoins);
    }

    void CoinAnimation()
    {
        if(coins.Length > 0)
        {
            foreach (var coin in coins)
            {
                coin.transform.DORotate(Vector3.right * 360, 3, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
            }
        }
    }

    void StartSettings()
    {
        CoinAnimation();

        TriggerWallActive(11);
    }

    void TriggerWallActive(int layerValue)
    {
        triggerWall.SetActive(true);
        triggerWall.GetComponent<TriggerWallAnimation>().WhichLayer = layerValue;
    }


}
