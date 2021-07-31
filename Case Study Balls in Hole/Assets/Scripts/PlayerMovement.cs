using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] Transform graphicTransform; // It's for shake object

    [SerializeField] float moveSpeed;
    [SerializeField] float deadZone = 50f; // Swipe deadzone

    bool isGameStarted = false;
    bool isMoving = false;

    RaycastHit stopWallRaycast;

    Vector3 direction;

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

            CheckMovingIfMovingIsNeeded();
        }

        #endregion

        #region  Swipe Control
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.deltaPosition.y > -deadZone && !isMoving)
            {
                direction = Vector3.back;
            }

            else if (touch.deltaPosition.x > -deadZone && !isMoving)
            {
                direction = Vector3.left;
            }

            else if (touch.deltaPosition.y > deadZone && !isMoving)
            {
                direction = Vector3.forward;
            }

            else if (touch.deltaPosition.x > deadZone && !isMoving)
            {
                direction = Vector3.right;
            }

            CheckMovingIfMovingIsNeeded();

        }
        #endregion
    }

    void CheckMovingIfMovingIsNeeded()
    {
        Physics.Raycast(transform.position,
                         direction, out stopWallRaycast, 100f); // Determine which wall will stop the ball or hole

        if(Vector3.Distance(transform.position, stopWallRaycast.point) < 0.5f) // If the ball is near the wall which is wanted by player, there is no need to go moving phase
        {
            isMoving = false;
        }

        else
        {
            
            isMoving = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

    private void FixedUpdate()
    {
        
        if (isMoving)
        {
            if (Vector3.Distance(transform.position, stopWallRaycast.point) > 0.5f)
            {
                StartGame();
                rb.velocity = moveSpeed * direction * Time.fixedDeltaTime;
            }

            else
            {
                graphicTransform.DOShakeScale(0.05f);
                isMoving = false;
                rb.constraints = RigidbodyConstraints.FreezeAll; // Not to have any bump physic effect
                stopWallRaycast = new RaycastHit(); // To absorb some confusions
            }
        }
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
