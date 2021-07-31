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

    private void Awake() 
    {
        Instance = this;
    }

    private void Start() 
    {
        triggerWall.SetActive(true);
        triggerWall.GetComponent<TriggerWallAnimation>().WallTag = "Wall";
    }

    public void StartTheGame()
    {
        
    }
}
