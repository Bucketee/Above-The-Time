using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicController : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;

    [Header("State")]
    [SerializeField] private Vector2 speed;
    [SerializeField] private bool isOnAir = false;
    [SerializeField] private bool timeLocked = false;

    [Header("Input")]
    [SerializeField] private InputController input = null;

    [Header("Move")]
    [SerializeField] private float walkSpeed = 6f;
    [SerializeField] private float runSpeed = 9f;

    [Header("Jump")]
    [SerializeField] private float jumpPower = 12f;
    [SerializeField] private float currJumpTime = 0f;
    [SerializeField] private float maxJumpTime = 0.2f;
    [SerializeField] private bool isJump = false;
    [SerializeField] private float fallAcceleration = 75f;
    [SerializeField] private float maxFallSpeed = -30f;
    [SerializeField] private List<LayerMask> stepableLayers = new List<LayerMask>();

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (timeLocked) return;
        CheckOnAir();
        CheckIsJump();
    }

    private void FixedUpdate()
    {
        if (timeLocked) return;
        HandleMove();
        HandleJump();
        ApplyMovement();
    }

    #region Move
    private void HandleMove()
    {
        var inputHor = input.RetrieveMoveInput();
        speed.x = inputHor * (input.RetrieveRunningInput() ? runSpeed : walkSpeed);
        if (inputHor >= 0f)
        {
            //right side
        }
        else
        {
            //left side
        }
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
        if (!isFloor || (isFloor && !stepableLayers.Contains(1 << GetFloor().layer)))
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
            if (currJumpTime < maxJumpTime) currJumpTime += Time.deltaTime;
            if (isOnAir && isJump && currJumpTime < maxJumpTime)
            {

            }
            else if (isOnAir && isJump && currJumpTime >= maxJumpTime)
            {
                speed.y = Mathf.MoveTowards(speed.y, maxFallSpeed, fallAcceleration * Time.deltaTime);
            }
            else if (!isOnAir && !isJump)
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
                speed.y = Mathf.MoveTowards(speed.y, maxFallSpeed, fallAcceleration * Time.deltaTime);
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
        var hit = Physics2D.Raycast(rigidbody2D.position, Vector2.down, 0.05f + rigidbody2D.transform.localScale.y / 2, ~(1 << gameObject.layer));
        Debug.DrawRay(rigidbody2D.position, Vector2.down * rigidbody2D.transform.localScale.y / 2, Color.green, 0.1f);
        return hit.collider;
    }

    private GameObject GetFloor()
    {
        var hit = Physics2D.Raycast(rigidbody2D.position, Vector2.down, 0.05f + rigidbody2D.transform.localScale.y / 2, ~(1 << gameObject.layer));
        return hit.collider.gameObject;
    }
    #endregion

    #region Time Lock 
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!timeLocked && collision.collider.GetComponentInParent<Bullet>() && collision.collider.GetComponentInParent<Bullet>().GetType() == Bullet.BulletType.time)
        {
            timeLocked = true;
            GetTimeLocked();
            Debug.Log("did it");
        }
    }

    private void GetTimeLocked()
    {
        speed = new Vector2(0, 0);
        rigidbody2D.velocity = new Vector2(0, 0);
    }
    #endregion
}
