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

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SlideToOne();
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
        beforeStartingUI.SetActive(false);
        inGameUI.SetActive(true);

        CanvasGroup canvas = inGameUI.GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        canvas.DOFade(1, 0.4f);
    }
}
