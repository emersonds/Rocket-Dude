using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
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

        Debug.Log($"Velocity: {rb2d.velocity.x}");

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

    }
}
