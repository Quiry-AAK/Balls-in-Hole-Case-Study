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
    bool tap,swipeLeft,swipeRight, swipeUp, swipeDown;
    bool isDragging = false;
    
    Vector2 startTouch, swipeDelta;
    float swipeDeadZone = 75f; 
    

    private void Update()
    {
        #region  For Unity Hub

        if (!isMoving)
        {
            if(Input.GetMouseButtonDown(0))
            {
                tap = true;
                startTouch = Input.mousePosition;
                isDragging = true;
            }

            else if(Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                Reset();
            }
        }

        #endregion

        #region  Swipe Control
        tap = swipeLeft = swipeRight = swipeUp = swipeDown;

        if(Input.touches.Length > 0 && !isMoving)
        {
            if(Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                startTouch = Input.touches[0].position;
                isDragging = true;
            }

            else if(Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDragging = false;
                Reset();
            }
        }
        #endregion

        swipeDelta = Vector2.zero;

        if(isDragging)
        {
            if(Input.touches.Length > 0)
            {
                swipeDelta = Input.touches[0].position - startTouch;
            }
            
            else if(Input.GetMouseButton(0))
            {
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
            }
        }

        if(swipeDelta.magnitude > swipeDeadZone && !isMoving)
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;

            if(Mathf.Abs(x) > Mathf.Abs(y))
            {
                if(x < 0)
                {
                    swipeLeft = true;
                    Direction = Vector3.left;
                }

                else
                {
                    swipeRight = true;
                    Direction = Vector3.right;
                }
            }

            else
            {
                if(y < 0)
                {
                    swipeDown = true;
                    Direction = Vector3.back;
                }

                else
                {
                    swipeUp = true;
                    Direction = Vector3.forward;
                }
            }
        }
    }

    private void Reset() 
    {
        startTouch = swipeDelta = Vector2.zero;
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
