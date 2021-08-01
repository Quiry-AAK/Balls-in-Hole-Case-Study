using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    // Singleton
    public static UIManager Instance;

    [SerializeField] Slider handSlider;
    [SerializeField] Text levelTxt;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject beforeStartingUI;
    [SerializeField] GameObject resultUI;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SlideToOne();
        SetLevelTxt();
    }

    void SlideToOne()
    {
        if (handSlider.gameObject.activeInHierarchy)
            handSlider.DOValue(1f, 0.8f, false).OnComplete(FakeLoop);
    }

    void FakeLoop()
    {
        if (handSlider.gameObject.activeInHierarchy)
        {
            handSlider.value = 0f;
            SlideToOne();
        }
    }

    void SetLevelTxt()
    {
        string a = SceneManager.GetActiveScene().name;
        int level = int.Parse(a.Remove(0, 5)); // Remove "Level" word in Scene name. It's int because I want to write 1 instead of 01.

        levelTxt.text = "Level " + level.ToString();
    }

    public void StartGameSettingsForUI()
    {
        if (GameManager.Instance.IsGameReadyToStart)
        {
            beforeStartingUI.SetActive(false);
            inGameUI.SetActive(true);

            CanvasGroup canvas = inGameUI.GetComponent<CanvasGroup>();
            canvas.alpha = 0;
            canvas.DOFade(1, 0.4f);
        }
    }

    public void FinishGameSettingsForUI(int totalCoins, int takenCoins)
    {
        inGameUI.SetActive(false);
        resultUI.SetActive(true);
        // TODO : Dotween canvas

        




        int resultPercent = 1;

        if(totalCoins != 0)
        {
            resultPercent = takenCoins / totalCoins;

            if(resultPercent >= 0 && resultPercent <= 0.33f)
            {
                // Not Bad and 1 star
            }

            else if(resultPercent > 0.33f && resultPercent <= 0.66f)
            {
                // Good and 2 star
            }

            else
            {
                // Awesome and 3 star
            }
        }
    }
}