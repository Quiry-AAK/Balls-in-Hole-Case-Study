using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float deadZone = 50f;

    bool isGameStarted = false;

    int whichDirection; // 0 : Down , 1 : Left , 2 : Up, 3 :  Right
    private void Update()
    {
        #region  For Unity Hub

        if (Input.anyKey)
        {
            if (Input.GetKey(KeyCode.S))
            {
                whichDirection = 0;
            }

            else if (Input.GetKey(KeyCode.A))
            {
                whichDirection = 1;
            }

            else if (Input.GetKey(KeyCode.W))
            {
                whichDirection = 2;
            }

            else if (Input.GetKey(KeyCode.D))
            {
                whichDirection = 3;
            }

            else
            {
                Debug.LogError("Use [W,A,S,D] for moving.");
                return;
            }

            Move();
        }

        #endregion

        #region  Swipe Control
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.deltaPosition.y > -deadZone)
            {
                whichDirection = 0;
            }

            else if (touch.deltaPosition.x > -deadZone)
            {
                whichDirection = 1;
            }

            else if (touch.deltaPosition.y > deadZone)
            {
                whichDirection = 2;
            }

            else if (touch.deltaPosition.x > deadZone)
            {
                whichDirection = 3;
            }

            Move();

        }
        #endregion
    }

    void Move()
    {
        StartGame();

        // TODO : Movement Codes
    }

    void StartGame()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            UIManager.Instance.StartGameSettingsForUI();
        }
    }
}
