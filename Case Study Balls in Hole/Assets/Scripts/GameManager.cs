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


    [Header ("Level Props")]
    public Color WallColor;
    public Color GroundColor;
    [SerializeField] List<GameObject> playerBalls;
    [SerializeField] GameObject playerHole;


    [Space]
    public GameObject TakeCoinFx;

    [HideInInspector] public bool IsGameReadyToStart = false;
    [HideInInspector] public bool isGameFinished = false;
    [HideInInspector] public int TakenCoins;
    [HideInInspector] public int TotalCoins;


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

        if (coins.Length > 0)
            TotalCoins = coins.Length;

        StartSettings();
    }



    public void BallAndHoleCollided()
    {
        capturedBalls++;

        if (capturedBalls >= balls.Length)
        {
            FinishGame();
        }
    }

    void FinishGame()
    {
        isGameFinished = true;
        Invoke("UIFinish", 1);

        if (TakenCoins != TotalCoins)
        {
            GameObject[] remainedCoins = GameObject.FindGameObjectsWithTag("Coin");

            StartCoroutine(DestroyCoinsInOrder(remainedCoins));
        }

        TriggerWallActive(12);
    }

    #region  Coin
    IEnumerator DestroyCoinsInOrder(GameObject[] remainedCoins)
    {
        foreach (var coin in remainedCoins)
        {
            yield return new WaitForSeconds(0.5f);
            CoinFxAndDestroy(coin);
        }
    }

    public void CoinFxAndDestroy(GameObject whichCoin)
    {
        if (TakeCoinFx.activeInHierarchy)
        {
            GameObject tempTakeCoinFx = Instantiate(TakeCoinFx);
            tempTakeCoinFx.transform.localScale = Vector3.one * 0.17f;
            tempTakeCoinFx.transform.position = whichCoin.transform.position;
            tempTakeCoinFx.transform.DOScale(0.25f, 0.3f).SetEase(Ease.InOutBack).OnComplete(CloseFX);
        }

        else
        {
            TakeCoinFx.SetActive(true);
            TakeCoinFx.transform.localScale = Vector3.one * 0.17f;
            TakeCoinFx.transform.position = whichCoin.transform.position;
            TakeCoinFx.transform.DOScale(0.25f, 0.3f).SetEase(Ease.InOutBack).OnComplete(CloseFX);
        }

        Destroy(whichCoin.transform.parent.gameObject);
    }

    void CloseFX()
    {
        TakeCoinFx.SetActive(false);
    }

    #endregion

    void UIFinish()
    {
        UIManager.Instance.FinishGameSettingsForUI(TotalCoins, TakenCoins);
    }

    void CoinAnimation()
    {
        if (coins.Length > 0)
        {
            foreach (var coin in coins)
            {
                coin.transform.DORotate(Vector3.right * 360, 3, RotateMode.FastBeyond360).SetEase(Ease.Linear).SetLoops(-1);
            }
        }
    }

    void StartSettings()
    {
        foreach (var item in playerBalls)
        {
            float ballYHolder = item.transform.position.y;
            item.transform.position = new Vector3(item.transform.position.x, 50, item.transform.position.z);
            item.transform.DOMoveY(ballYHolder, 0.7f);
        }

        float holeYHolder = playerHole.transform.position.y;
        playerHole.transform.position = new Vector3(playerHole.transform.position.x, -9.5f, playerHole.transform.position.z);
        playerHole.transform.DOMoveY(holeYHolder, 0.7f);


        CoinAnimation();

        TriggerWallActive(11);
    }

    void TriggerWallActive(int layerValue)
    {
        triggerWall.SetActive(true);
        triggerWall.GetComponent<TriggerWallAnimation>().WhichLayer = layerValue;
    }


}
