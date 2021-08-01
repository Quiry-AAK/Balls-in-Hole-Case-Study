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
    [SerializeField] Text complimentTxt;
    [SerializeField] GameObject inGameUI;
    [SerializeField] GameObject beforeStartingUI;
    [SerializeField] GameObject resultUI;
    [SerializeField] ParticleSystem starFx;
    [SerializeField] CanvasGroup[] stars;


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

        resultUI.GetComponent<CanvasGroup>().DOFade(1, 0.2f);


        if(totalCoins != 0)
        {
            int resultPercent = takenCoins / totalCoins;

            if(resultPercent >= 0 && resultPercent <= 0.33f)
            {
                Result("NOT BAD", 1);
            }

            else if(resultPercent > 0.33f && resultPercent <= 0.66f)
            {
                Result("GOOD", 2);
            }

            else
            {
                Result("AWESOME", 3);
            }
        }

        else
        {
            Result("AWESOME", 3);
        }
    }

    void Result(string text, int count)
    {
        complimentTxt.text = "AWESOME";
        StartCoroutine(ScaleAndFxStars(count));
    }

    IEnumerator ScaleAndFxStars(int count)
    {
        for (int i = 0; i < count; i++)
        {
            yield return  new WaitForSeconds(0.5f);
            stars[i].DOFade(1, 0.5f);
            FxStars(stars[i].GetComponent<RectTransform>());
        }
    }

    void FxStars(RectTransform starTransform)
    {
        starFx.gameObject.SetActive(true);
        starFx.transform.position = starTransform.position;
        starFx.Play();

        Invoke("CloseFx",3);
    }

    void CloseFx()
    {
        starFx.gameObject.SetActive(false);
    }

    public void RestartBtn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GoNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}