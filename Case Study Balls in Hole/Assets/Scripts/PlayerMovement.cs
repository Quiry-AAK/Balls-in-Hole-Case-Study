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
    [SerializeField] LayerMask maskLayer;

    [Space]
    [SerializeField] bool isItBall;


    RaycastHit stopWallRaycast;

    public Vector3 Direction;

    bool isGameStarted = false;
    bool isMoving = false;
    bool doStartMoving = false;

    private void Update()
    {
        #region  For Unity Hub

        if (Input.anyKey && !isMoving)
        {
            if (Input.GetKey(KeyCode.S))
            {
                Direction = Vector3.back;
            }

            else if (Input.GetKey(KeyCode.A) && !isMoving)
            {
                Direction = Vector3.left;
            }

            else if (Input.GetKey(KeyCode.W) && !isMoving)
            {
                Direction = Vector3.forward;
            }

            else if (Input.GetKey(KeyCode.D) && !isMoving)
            {
                Direction = Vector3.right;
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
                Direction = Vector3.back;
            }

            else if (touch.deltaPosition.x > -swipeDeadZone && !isMoving)
            {
                Direction = Vector3.left;
            }

            else if (touch.deltaPosition.y > swipeDeadZone && !isMoving)
            {
                Direction = Vector3.forward;
            }

            else if (touch.deltaPosition.x > swipeDeadZone && !isMoving)
            {
                Direction = Vector3.right;
            }
        }
        #endregion
    }

    public void CheckMovingIfMovingIsNeeded()
    {
        if (GameManager.Instance.IsGameReadyToStart && Direction != Vector3.zero && !GameManager.Instance.isGameFinished)
        {
            if (Physics.Raycast(transform.position,
                             Direction, out stopWallRaycast, 100f, ~maskLayer))
            {

                if (Vector3.Distance(transform.position, stopWallRaycast.point) < movingDeadZone) // If the ball/hole is near the wall which is wanted by player, there is no need to go moving phase
                {
                    isMoving = false;
                    rb.constraints = RigidbodyConstraints.FreezeAll; // Not to have any bump physic effect

                    if (isItBall && stopWallRaycast.collider.gameObject.layer == 16)
                        GetComponent<Ball>().CollideGlass(stopWallRaycast.collider.gameObject, stopWallRaycast.transform.position);

                    stopWallRaycast = new RaycastHit(); // To absorb some confusions
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

        else
        {
            isMoving = false;
            rb.velocity = Vector3.zero;

            rb.constraints = RigidbodyConstraints.FreezeAll; // Not to have any bump physic effect

            if (doStartMoving)
            {
                doStartMoving = false;
                if (isItBall)
                    graphicTransform.DOScale(1.2f, 0.07f).OnComplete(ScaleToOneAgain);
            }
        }
    }

    void ScaleToOneAgain()
    {
        graphicTransform.DOScale(1, 0.07f);
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
        rb.velocity = moveSpeed * Direction * Time.fixedDeltaTime;

        if (isItBall)
        {
            rb.rotation = Quaternion.Euler(rb.rotation.x + (Direction.x * 50f), rb.rotation.y + (Direction.y * 50f), rb.rotation.z + (Direction.z * 50f));
        }
    }
}
