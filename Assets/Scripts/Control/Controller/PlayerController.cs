using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;

    [Header("State")]
    [SerializeField] private Vector2 speed;
    [SerializeField] private bool isOnAir = false;

    [Header("Input")]
    [SerializeField] private InputController input = null;

    [Header("Move")]
    [SerializeField] private float walkSpeed = 6f;

    [Header("Dash")]
    [SerializeField] private float dashDistance;

    [Header("Jump")]
    [SerializeField] private float jumpPower = 12f;
    [SerializeField] private float currJumpTime = 0f;
    [SerializeField] private float maxJumpTime = 0.2f;
    [SerializeField] private bool isJump = false; //is jumping
    [SerializeField] private float fallAcceleration = 75f;
    [SerializeField] private float maxFallSpeed = -30f;
    [SerializeField] private LayerMask stepableLayers;
    [SerializeField] private LayerMask collidingLayers; //head
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        CheckOnAir();
        CheckIsJump();
    }

    private void FixedUpdate()
    {
        HandleMove();
        HandleJump();
        ApplyMovement();
    }

    #region Move
    private void HandleMove()
    {
        float inputHor = input.RetrieveMoveInput();
        speed.x = inputHor * walkSpeed;
        if (inputHor >= 0f)
        {
            //right side help
        }
        else
        {
            //left side
        }
    }
    #endregion

    #region Dash
    private void HandleDash()
    {
        float inputHor = input.RetrieveMoveInput();
        bool dashInput = input.RetrieveDashInput();
        return; //help
    }
    #endregion

    #region Jump
    /// <summary>
    /// make isJump to false from true when is on floor
    /// </summary>
    private void CheckIsJump()
    {
        if (isJump && !isOnAir)
        {
            isJump = false;
        }
    }

    /// <summary>
    /// change state of isOnAir by CheckFloor()
    /// </summary>
    private void CheckOnAir()
    {
        bool isFloor = CheckFloor();
        if (!isFloor)
        {
            isOnAir = true;
        }
        else
        {
            isOnAir = false;
        }
    }

    private void HandleJump()
    {
        if (input.RetrieveJumpInput() && !isJump)
        {
            if (!isOnAir)
            {
                speed.y = jumpPower;
                isJump = true;
                currJumpTime = 0f;
            }
        }
        else if (input.RetrieveJumpHoldInput())
        {
            if (currJumpTime < maxJumpTime) currJumpTime += Time.fixedDeltaTime;
            if (isOnAir && isJump && currJumpTime < maxJumpTime)
            {
                //long jump
            }
            else if (isOnAir && isJump && currJumpTime >= maxJumpTime)
            {
                speed.y = Mathf.MoveTowards(speed.y, maxFallSpeed, fallAcceleration * Time.fixedDeltaTime);
            }
            else if (!isOnAir && !isJump)
            {
                speed.y = jumpPower;
                isJump = true;
                currJumpTime = 0f;
            }
            else if (isOnAir && !isJump )
            {
                speed.y = Mathf.MoveTowards(speed.y, maxFallSpeed, fallAcceleration * Time.fixedDeltaTime);
            }
        }
        else if (input.RetrieveJumpUpInput())
        {
            currJumpTime = maxJumpTime;
        }
        else
        {
            if (isOnAir)
            {
                if (currJumpTime < maxJumpTime) currJumpTime = maxJumpTime;
                speed.y = Mathf.MoveTowards(speed.y, maxFallSpeed, fallAcceleration * Time.fixedDeltaTime);
            }
            else
            {
                speed.y = 0;
            }
        }

        if (isJump && CheckHead())
        {
            if (currJumpTime < maxJumpTime)
            {
                currJumpTime = maxJumpTime;
                speed.y = 0;
            }
        }
    }
    #endregion

    #region Movement
    private void ApplyMovement()
    {
        rigidbody2D.velocity = speed;
    }
    #endregion

    #region Floor
    private bool CheckFloor()
    {
        var hit = Physics2D.Raycast(rigidbody2D.position - new Vector2(rigidbody2D.transform.localScale.x / 2, rigidbody2D.transform.localScale.y / 2 + 0.05f), Vector2.right, rigidbody2D.transform.localScale.x, stepableLayers);
        Debug.DrawRay(rigidbody2D.position - new Vector2(rigidbody2D.transform.localScale.x / 2, rigidbody2D.transform.localScale.y / 2 + 0.05f), Vector2.right * rigidbody2D.transform.localScale.x, Color.green, 0.1f);
        return hit.collider;
    }

    private GameObject GetFloor()
    {
        var hit = Physics2D.Raycast(rigidbody2D.position, Vector2.down, 0.05f + rigidbody2D.transform.localScale.y / 2, stepableLayers);
        return hit.collider.gameObject;
    }
    #endregion

    #region Head
    private bool CheckHead()
    {
        var hit = Physics2D.Raycast(rigidbody2D.position - new Vector2(rigidbody2D.transform.localScale.x / 2, -rigidbody2D.transform.localScale.y / 2 - 0.05f), Vector2.right, rigidbody2D.transform.localScale.x, collidingLayers);
        Debug.DrawRay(rigidbody2D.position - new Vector2(rigidbody2D.transform.localScale.x / 2, -rigidbody2D.transform.localScale.y / 2 - 0.05f), Vector2.right * rigidbody2D.transform.localScale.x, Color.red, 0.1f);
        return hit.collider;
    }
    #endregion
}
