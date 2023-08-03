using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAController : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;

    [Header("State")]
    [SerializeField] private Vector2 speed;
    [SerializeField] private bool isOnAir = false;

    [Header("Input")]
    [SerializeField] private InputController input = null;

    [Header("Move")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool movingCool = false;
    private float originX;
    [SerializeField] private float leftDisX;
    [SerializeField] private float rightDisX;
    [SerializeField] private float destinationX;
    [SerializeField] private float horizontalDir;

    [Header("Jump")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float currJumpTime;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private bool isJump = false;
    [SerializeField] private float fallAcceleration;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private LayerMask stepableLayers;

    private TimeLockObject timeLockObject;


    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        timeLockObject = GetComponent<TimeLockObject>();
        leftDisX = transform.position.x - leftDisX;
        rightDisX = transform.position.x + rightDisX;
    }

    private void Update()
    {
        CheckOnAir();
        CheckIsJump();
    }

    private void FixedUpdate()
    {
        if (timeLockObject.TimeLocked)
        {
            return;
        }
        HandleMove();
        HandleJump();
        ApplyMovement();
    }

    #region Move
    private void HandleMove()
    {
        if (isMoving && Mathf.Abs(transform.position.x - destinationX) <= 0.1)
        {
            isMoving = false;
            speed.x = 0f;
            StartCoroutine(MovingCoolCo());
        }
        if (isMoving || movingCool) return;

        destinationX = Random.Range(leftDisX, rightDisX);
        horizontalDir = destinationX < transform.position.x ? -1f : 1f;
        speed.x = horizontalDir * walkSpeed;
        isMoving = true;

        if (horizontalDir >= 0f)
        {
            //right side
        }
        else
        {
            //left side
        }
    }

    private IEnumerator MovingCoolCo()
    {
        movingCool = true;
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        movingCool = false;
    }
    #endregion

    #region Jump
    private void CheckIsJump()
    {
        if (isJump && !isOnAir) //add floor.layer to check floor type help
        {
            isJump = false;
        }
    }

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
        else
        {
            if (isOnAir)
            {
                speed.y = Mathf.MoveTowards(speed.y, maxFallSpeed, fallAcceleration * Time.fixedDeltaTime);
            }
            else
            {
                speed.y = 0;
            }
        }

        if (input.RetrieveJumpUpInput())
        {
            currJumpTime = maxJumpTime;
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
        var hit = Physics2D.Raycast(rigidbody2D.position, Vector2.down, 0.05f + rigidbody2D.transform.localScale.y / 2, stepableLayers); //need revise to use
        return hit.collider.gameObject;
    }
    #endregion

}
