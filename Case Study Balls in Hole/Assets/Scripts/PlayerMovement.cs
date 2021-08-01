using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform graphicTransform; // It's for shake object

    [Space]
    [SerializeField] float moveSpeed;
    [SerializeField] float swipeDeadZone = 50f; // Swipe deadzone
    public float movingDeadZone;

    [Space]
    [SerializeField] bool isItBall;


    RaycastHit stopWallRaycast;

    Vector3 direction;

    bool isGameStarted = false;
    bool isMoving = false;
    bool doStartMoving = false;

    float scaleCheck;

    private void Update()
    {
        #region  For Unity Hub

        if (Input.anyKey && !isMoving)
        {
            if (Input.GetKey(KeyCode.S))
            {
                direction = Vector3.back;
            }

            else if (Input.GetKey(KeyCode.A) && !isMoving)
            {
                direction = Vector3.left;
            }

            else if (Input.GetKey(KeyCode.W) && !isMoving)
            {
                direction = Vector3.forward;
            }

            else if (Input.GetKey(KeyCode.D) && !isMoving)
            {
                direction = Vector3.right;
            }

            else
            {
                Debug.LogError("Use [W,A,S,D] for moving.");
                return;
            }
        }

        #endregion

        #region  Swipe Control
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.deltaPosition.y > -swipeDeadZone && !isMoving)
            {
                direction = Vector3.back;
            }

            else if (touch.deltaPosition.x > -swipeDeadZone && !isMoving)
            {
                direction = Vector3.left;
            }

            else if (touch.deltaPosition.y > swipeDeadZone && !isMoving)
            {
                direction = Vector3.forward;
            }

            else if (touch.deltaPosition.x > swipeDeadZone && !isMoving)
            {
                direction = Vector3.right;
            }
        }
        #endregion
    }

    public void CheckMovingIfMovingIsNeeded()
    {
        if (GameManager.Instance.IsGameReadyToStart && direction != Vector3.zero)
        {
            Physics.Raycast(transform.position,
                             direction, out stopWallRaycast, 100f, ~13);

            if (Vector3.Distance(transform.position, stopWallRaycast.point) < movingDeadZone) // If the ball/hole is near the wall which is wanted by player, there is no need to go moving phase
            {
                isMoving = false;
                rb.constraints = RigidbodyConstraints.FreezeAll; // Not to have any bump physic effect
                stopWallRaycast = new RaycastHit(); // To absorb some confusions

                if (doStartMoving)
                {
                    doStartMoving = false;
                    if (isItBall)
                        graphicTransform.DOShakeScale(0.3f);
                }
            }

            else
            {
                isMoving = true;
                doStartMoving = true;
                if (isItBall)
                    rb.constraints = RigidbodyConstraints.FreezePositionY;
                else
                    rb.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

                Move();
            }
        }
    }

    private void FixedUpdate()
    {
        CheckMovingIfMovingIsNeeded();
    }

    void StartGame()
    {
        if (!isGameStarted)
        {
            isGameStarted = true;
            UIManager.Instance.StartGameSettingsForUI();
        }
    }

    void Move()
    {
        StartGame();
        rb.velocity = moveSpeed * direction * Time.fixedDeltaTime;
    }
}
