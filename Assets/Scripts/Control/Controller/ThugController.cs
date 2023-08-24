using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThugController : MonoBehaviour
{
    private new Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;

    [SerializeField] private Player player;
    [SerializeField] private Collider2D playerDetectCollider2D;
    [SerializeField] private Collider2D attackCollider2D;
    private float attackBeforeWaitingTime = 1f;
    private float attackAfterWaitingTime = 0.3f;

    [Header("State")]
    [SerializeField] private Vector2 speed;
    [SerializeField] private bool isOnAir = false;
    [SerializeField] private bool detectPlayer;
    [SerializeField] private bool detectCliff;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isComingBack = false;

    [Header("Input")]
    [SerializeField] private InputController input = null;

    [Header("Move")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;
    [SerializeField] private bool movingCool = false;
    [SerializeField] private float normalLeftX;
    [SerializeField] private float normalRightX;
    [SerializeField] private float chaseLeftX;
    [SerializeField] private float chaseRightX;
    private float normalRangeLeftX;
    private float normalRangeRightX;
    private float chaseRangeLeftX;
    private float chaseRangeRightX;
    private float detectDistanceX;
    [SerializeField] private float destinationX;
    [SerializeField] private bool hasDestination = false;
    [SerializeField] private float horizontalDir;

    [Header("Jump")]
    [SerializeField] private float jumpPower;
    [SerializeField] private float currJumpTime;
    [SerializeField] private float maxJumpTime;
    [SerializeField] private bool isJump = false;
    [SerializeField] private float fallAcceleration;
    [SerializeField] private float maxFallSpeed;
    [SerializeField] private LayerMask stepableLayers;

    private void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    
        float nowPositionX = transform.position.x;
        normalRangeLeftX = nowPositionX - normalLeftX;
        normalRangeRightX = nowPositionX + normalRightX;
        chaseRangeLeftX = nowPositionX - chaseLeftX;
        chaseRangeRightX = nowPositionX + chaseRightX;

        detectDistanceX = playerDetectCollider2D.gameObject.transform.localScale.x/2f;
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
        if (isAttacking)
        {
            return;
        }

        if (isJump)
        {
            return;
        }

        if (transform.position.x < chaseRangeLeftX || transform.position.x > chaseRangeRightX)
        {
            Debug.Log("Start Comeback");
            detectPlayer = false;
            isComingBack = true;
            FindDestination();
            SetVelocity();
            return;
        }

        if (detectPlayer && CheckDetectPlayerX())
        {
            detectPlayer = true;
            movingCool = false;
            FindDestination();
            if (CheckPlayerInAttackCollider())
            {
                speed.x = 0f;
                isAttacking = true;
                StartCoroutine(AttackCo());
            }
            else
            {
                FindDestination();
                SetVelocity();
            }
        }
        else if (detectPlayer)
        {
            Debug.Log("Start Comeback");
            detectPlayer = false;
            isComingBack = true;
            FindDestination();
            SetVelocity();
        }
        else if (!isComingBack && CheckDetectPlayer())
        {
            Debug.Log("Detect Player");
            detectPlayer = true;
            movingCool = false;
            FindDestination();
            if (CheckPlayerInAttackCollider())
            {
                speed.x = 0f;
                isAttacking = true;
                StartCoroutine(AttackCo());
            }
            else
            {
                FindDestination();
                SetVelocity();
            }
        }
        else if (movingCool)
        {
            return;
        }
        else if (!hasDestination)
        {
            FindDestination();
        }
        else
        {
            if (CheckNearDestination())
            {
                hasDestination = false;
                isComingBack = false;
                speed.x = 0f;
                StartCoroutine(MovingCoolCo());
            }
            else
            {
                SetVelocity();
            }
        }
    }
    private bool CheckDetectPlayer()
    {
        bool isNearPlayer = false;
        List<Collider2D> overlappedColliders = new();
        playerDetectCollider2D.OverlapCollider(new ContactFilter2D().NoFilter(), overlappedColliders);
        foreach(Collider2D c in overlappedColliders)
        {
            if (c.gameObject.layer == 3)
            {
                isNearPlayer = true;
            }
        }
        return isNearPlayer;
    }
    private bool CheckDetectPlayerX()
    {
        return (Mathf.Abs(player.transform.position.x - transform.position.x) < detectDistanceX);
    }
    private bool CheckNearDestination()
    {
    
        return (Mathf.Abs(transform.position.x - destinationX) <= 0.1f);
    }
    private bool CheckPlayerInAttackCollider()
    {
        bool isNearPlayer = false;
        List<Collider2D> overlappedColliders = new();
        attackCollider2D.OverlapCollider(new ContactFilter2D().NoFilter(), overlappedColliders);
        foreach(Collider2D c in overlappedColliders)
        {
            if (c.gameObject.layer == 3)
            {
                isNearPlayer = true;
            }
        }
        return isNearPlayer;

    }
    private void SetVelocity()
    {
        horizontalDir = destinationX < transform.position.x ? -1f : 1f;
        if (horizontalDir >= 0f)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
        speed.x = horizontalDir * walkSpeed;
    }

    private IEnumerator AttackCo()
    {
        Debug.Log("Start Attack");
        yield return new WaitForSeconds(attackBeforeWaitingTime);
        if (CheckPlayerInAttackCollider())
        {
            Debug.Log("GetAttacked");
            // player.getAttack();
        }
        yield return new WaitForSeconds(attackAfterWaitingTime);
        Debug.Log("End Attack");
        isAttacking = false;
    }

    private IEnumerator MovingCoolCo()
    {
        movingCool = true;
        yield return new WaitForSeconds(Random.Range(2f, 5f));
        movingCool = false;
    }

    private void FindDestination()
    {
        if (detectPlayer && !isComingBack)
        {
            destinationX = player.transform.position.x;   
        }
        else
        {
            destinationX = Random.Range(normalRangeLeftX, normalRangeRightX);
        }
        hasDestination = true;
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
        if (CheckCliff() && !isJump)
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

    private bool CheckCliff()
    {
        var leftHit = Physics2D.Raycast(rigidbody2D.position - new Vector2(rigidbody2D.transform.localScale.x / 2 - 0.01f, rigidbody2D.transform.localScale.y / 2  + 0.02f), Vector2.down, 0.08f, stepableLayers);
        var righttHit = Physics2D.Raycast(rigidbody2D.position - new Vector2(- rigidbody2D.transform.localScale.x / 2 + 0.01f, rigidbody2D.transform.localScale.y / 2 + 0.02f), Vector2.down, 0.08f, stepableLayers);
        Debug.DrawRay(rigidbody2D.position - new Vector2(rigidbody2D.transform.localScale.x / 2 - 0.01f, rigidbody2D.transform.localScale.y / 2 - 0.02f), Vector2.down * 0.08f, Color.green, 0.1f);
        Debug.DrawRay(rigidbody2D.position - new Vector2(- rigidbody2D.transform.localScale.x / 2 + 0.01f, rigidbody2D.transform.localScale.y / 2 - 0.02f), Vector2.down * 0.08f, Color.green, 0.1f);
        bool leftBool = leftHit.collider;
        bool rightBool = righttHit.collider;
        if (horizontalDir >= 0f)
        {
            detectCliff = (!rightBool && leftBool);
            return (!rightBool && leftBool);
        }
        else
        {
            detectCliff = (!leftBool && rightBool);
            return (!leftBool && rightBool);
        }
    }

    private GameObject GetFloor()
    {
        var hit = Physics2D.Raycast(rigidbody2D.position, Vector2.down, 0.05f + rigidbody2D.transform.localScale.y / 2, stepableLayers); //need revise to use
        return hit.collider.gameObject;
    }
    #endregion
}
