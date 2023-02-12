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
        // Assign component references
        stats = GetComponent<PlayerStats>();
        rb2d = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        // Track player input
        GetInput();

        // Fall when not grounded
        Fall();
    }

    /// <summary>
    /// Handles all inputs to trigger actions when required.
    /// </summary>
    private void GetInput()
    {
        // Get horizontal input
        hInput = Input.GetAxisRaw("Horizontal") * stats.MoveSpeed;

        // Get jump input
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        // Get shoot input
        if (Input.GetMouseButtonDown(0))
        {
            GetComponentInChildren<RocketController>().Shoot();
        }
    }

    // FixedUpdate is called every fixed framerate frame, used for calculating physics.
    private void FixedUpdate()
    {
        // Moves the player according to hInput
        Move();
    }

    /// <summary>
    /// Moves the player using physics forces.
    /// </summary>
    private void Move()
    {
        // Uses hInput to add an impulse (instant) force to the player for movement
        rb2d.AddForce(new Vector2(hInput, 0f) * Time.deltaTime, ForceMode2D.Impulse);
    }

    /// <summary>
    /// Makes the player jump if they are grounded or will be grounded within jumpSaverTime seconds.
    /// </summary>
    private void Jump()
    {
        // Player is grounded and can jump
        if (IsGrounded())
        {
            rb2d.AddForce(new Vector2(0f, 1f) * stats.JumpForce, ForceMode2D.Impulse);
        }
        // Player is not grounded, but is provided a grace period for more consistent jumps.
        else
        {
            StartCoroutine(JumpSaver(jumpSaverTime));
        }
    }

    /// <summary>
    /// Accelerates the player's gravity while falling for smoother falling.
    /// </summary>
    private void Fall()
    {
        if (rb2d.velocity.y < 0)
        {
            rb2d.velocity += Vector2.up * Physics2D.gravity.y * (stats.FallMultiplier - 1f) * Time.deltaTime;
        }
    }

    /// <summary>
    /// Checks if the player is grounded by casting a box at their feet and checking for collision.
    /// </summary>
    /// <returns></returns>
    private bool IsGrounded()
    {
        // Casts a box at the player's feet to determine if they are colliding with ground
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

        // Return true if colliding with ground
        return hit.collider != null;
    }

    /// <summary>
    /// Grace period after inputting a jump action before landing to make jumps feel more consistent. 
    /// If the player lands on the ground before grace period ends, they will still jump despite attempting to jump too early.
    /// </summary>
    /// <param name="timeToWait">Time window to be grounded for a late jump.</param>
    /// <returns></returns>
    private IEnumerator JumpSaver(float timeToWait)
    {
        // Decrement time to wait until grace period is over
        while (timeToWait > 0)
        {
            // If grounded, jump and break out of while loop.
            if (IsGrounded())
            {
                // Late jump
                rb2d.AddForce(new Vector2(0f, 1f) * stats.JumpForce, ForceMode2D.Impulse);
                yield break;
            }
            else
            {
                // Wait approximately one frame before checking for landing
                yield return new WaitForSeconds(0.02f);

                // Decrement timeToWait grace period
                timeToWait -= 0.02f;
            }
        }
    }
}
