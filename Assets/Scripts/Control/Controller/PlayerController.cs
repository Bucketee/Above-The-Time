using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    [SerializeField] private BoxCollider2D boxCollider2D;

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

    [Header("Dash")]
    [SerializeField] private bool dashUsed = false; //dash used
    [SerializeField] private bool isDash = false; //is dashing
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashTime;
    [SerializeField] private float dashAcceleration;
    private bool dashInput;

    private SoundManager soundManager;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>(false);
    }

    private void Start()
    {
        soundManager = SoundManager.Instance;
    }
    private void Update()
    {
        CheckOnAir();
        CheckIsJump();
        dashInput = input.RetrieveDashInput();
        if (!isDash )
        {
            HandleDash();
        }
    }

    private void FixedUpdate()
    {
        if (!isDash)
        {
            HandleMove();
            HandleJump();
        }
        ApplyMovement();
    }

    #region Move
    private void HandleMove()
    {
        float inputHor = input.RetrieveMoveInput();
        speed.x = inputHor * walkSpeed;
        if (inputHor > 0f)
        {
            spriteRenderer.flipX = false;
            animator.SetBool("isMoving", true);
        }
        else if (inputHor < 0f)
        {
            spriteRenderer.flipX = true;
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
    }
    #endregion

    #region Dash
    private void HandleDash()
    {
        float inputHor = input.RetrieveMoveInput();

        if(inputHor != 0f && dashInput && !dashUsed) 
        {
            isDash = true;
            dashUsed = true;
            StartCoroutine(Dash(inputHor));
        }
    }

    IEnumerator Dash(float inputHor)
    {
        float maxFallSpeedSet = maxFallSpeed;
        maxFallSpeed = 0f;
        speed.y = 0f;
        if(inputHor > 0f)
        {
            speed.x = 0.1f;
            while (speed.x < dashSpeed)
            {
                Debug.Log("Accelerating");
                if (speed.x == 0f) { break; }
                speed.x = Mathf.MoveTowards(speed.x, dashSpeed, dashAcceleration * Time.fixedDeltaTime);
                speed.y = 0f;
                yield return new WaitForFixedUpdate();
            }
        }
        else if (inputHor < 0f)
        {
            speed.x = -0.1f;
            while (speed.x > -dashSpeed)
            {
                Debug.Log("Accelerating");
                if (speed.x == 0f) { break; }
                speed.x = Mathf.MoveTowards(speed.x, -dashSpeed, dashAcceleration * Time.fixedDeltaTime);
                speed.y = 0f;
                yield return new WaitForFixedUpdate();
            }
        }
        yield return new WaitForSeconds(dashTime);
        if (inputHor > 0f)
        {
            while (speed.x > 0f)
            {
                Debug.Log("Decelerating");
                speed.x = Mathf.MoveTowards(speed.x, 0f, dashAcceleration * Time.fixedDeltaTime);
                speed.y = 0f;
                yield return new WaitForFixedUpdate();
            }
        }
        else if (inputHor < 0f)
        {
            while (speed.x < 0f)
            {
                Debug.Log("Decelerating");
                speed.x = Mathf.MoveTowards(speed.x, 0f, dashAcceleration * Time.fixedDeltaTime);
                speed.y = 0f;
                yield return new WaitForFixedUpdate();
            }
        }

        isDash = false;
        maxFallSpeed = maxFallSpeedSet;
        yield return null;
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
            dashUsed = false;
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
        var hit = Physics2D.Raycast(rigidbody2D.position - new Vector2(rigidbody2D.transform.localScale.x * boxCollider2D.size.x / 2, rigidbody2D.transform.localScale.y * boxCollider2D.size.y / 2 + 0.05f), Vector2.right, rigidbody2D.transform.localScale.x * boxCollider2D.size.x, stepableLayers);
        Debug.DrawRay(rigidbody2D.position - new Vector2(rigidbody2D.transform.localScale.x * boxCollider2D.size.x / 2, rigidbody2D.transform.localScale.y * boxCollider2D.size.y / 2 + 0.05f), Vector2.right * rigidbody2D.transform.localScale.x * boxCollider2D.size.x, Color.green, 0.1f);
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
        var hit = Physics2D.Raycast(rigidbody2D.position - new Vector2(rigidbody2D.transform.localScale.x * boxCollider2D.size.x / 2, -rigidbody2D.transform.localScale.y * boxCollider2D.size.y / 2 - 0.05f), Vector2.right, rigidbody2D.transform.localScale.x * boxCollider2D.size.x, collidingLayers);
        Debug.DrawRay(rigidbody2D.position - new Vector2(rigidbody2D.transform.localScale.x * boxCollider2D.size.x / 2, -rigidbody2D.transform.localScale.y * boxCollider2D.size.y / 2 - 0.05f), Vector2.right * rigidbody2D.transform.localScale.x * boxCollider2D.size.x, Color.red, 0.1f);
        return hit.collider;
    }
    #endregion
}
