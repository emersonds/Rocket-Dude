using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Fields

    private bool canJump = true;    // If the player can jump
    private bool grounded = true;   // If the player is on ground

    #endregion

    #region Components

    // These are components attached to the player object

    private PlayerStats stats;
    private Rigidbody2D rb2d;
    private SpriteRenderer spriteRenderer;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        stats = GetComponent<PlayerStats>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Jump();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        rb2d.AddForce(new Vector2(Input.GetAxisRaw("Horizontal"), 0f) * stats.MoveSpeed, ForceMode2D.Impulse);

        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            transform.localScale = Vector3.one;
        }
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canJump && grounded)
        {
            Debug.Log("Jump!");

            rb2d.AddForce(new Vector2(0f, 1f) * stats.JumpForce, ForceMode2D.Impulse);

            StartCoroutine(JumpCooldown());
        }
    }

    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(0.7f);
        canJump = true;
    }
}
