using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields

    //private bool canJump = true;    // If the player can jump

    [SerializeField, Tooltip("Extra distance from bottom of collider that is checked for ground/jumps.")]
    private float groundedBuffer = 0.5f;

    [SerializeField, Tooltip("How long to wait before jumping or not when the player jumps as they land.")]
    private float jumpSaverTime = 0.25f;

    private float hInput = 0f;      // Where the player is moving

    #endregion

    #region Components

    // These are components attached to the player object

    private CapsuleCollider2D capsuleCollider;
    private PlayerStats stats;
    private Rigidbody2D rb2d;

    #endregion

    [SerializeField, Tooltip("Ground layer for grounded checking and jumping/falling.")]
    private LayerMask isGround;

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        hInput = Input.GetAxisRaw("Horizontal") * stats.MoveSpeed;

        Jump();
        Fall();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb2d.AddForce(new Vector2(hInput, 0f) * Time.deltaTime, ForceMode2D.Impulse);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Debug.Log("Jump!");

            rb2d.AddForce(new Vector2(0f, 1f) * stats.JumpForce, ForceMode2D.Impulse);
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !IsGrounded())
        {
            StartCoroutine(JumpSaver(jumpSaverTime));
        }
    }

    private void Fall()
    {
        if (rb2d.velocity.y < 0)
        {
            Debug.Log("Fall!");
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (stats.FallMultiplier - 1) * Time.deltaTime;

            if (IsGrounded())
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0f);
            }
        }
    }

    private bool IsGrounded()
    {
        RaycastHit2D hit =
            Physics2D.BoxCast(capsuleCollider.bounds.center,
                              capsuleCollider.bounds.size - new Vector3(0.1f, 0f, 0f),
                              0f, Vector2.down, groundedBuffer, isGround);

        // This was used for visualizing the box cast
        /*
        Color rayColor;
        if (hit.collider != null)
        {
            rayColor = Color.green;
        }
        else
        {
            rayColor = Color.red;
        }
        Debug.DrawRay(capsuleCollider.bounds.center + new Vector3(capsuleCollider.bounds.extents.x, 0), Vector2.down * (capsuleCollider.bounds.extents.y + groundedBuffer), rayColor);
        Debug.DrawRay(capsuleCollider.bounds.center - new Vector3(capsuleCollider.bounds.extents.x, 0), Vector2.down * (capsuleCollider.bounds.extents.y + groundedBuffer), rayColor);
        Debug.Log(hit.collider);
        */

        return hit.collider != null;
    }

    private IEnumerator JumpSaver(float timeToWait)
    {
        while (timeToWait > 0)
        {
            if (IsGrounded())
            {
                Debug.Log("Late jump!");
                rb2d.AddForce(new Vector2(0f, 1f) * stats.JumpForce, ForceMode2D.Impulse);
                yield break;
            }
            else
            {
                Debug.Log("No ground!");
                yield return new WaitForSeconds(0.02f); // Wait approximately one frame
                timeToWait -= 0.02f;
            }
        }
    }
}
