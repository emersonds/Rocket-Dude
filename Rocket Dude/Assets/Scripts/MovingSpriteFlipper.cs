using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSpriteFlipper : MonoBehaviour
{
    // Rigidbody component of the parent for velocity checking
    private Rigidbody2D rb2dParent;

    // SpriteRender component of the sprite being flipped
    private SpriteRenderer sr;

    // Start is called before the first frame update
    private void Start()
    {
        // Assign component references
        sr = GetComponent<SpriteRenderer>();
        rb2dParent = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update()
    {
        Flip();
    }

    /// <summary>
    /// Flips the sprite horizontally depending on horizontal movement.
    /// </summary>
    private void Flip()
    {
        // Confirm the parent object has a RigidBody2D component
        if (rb2dParent != null)
        {
            // Flip according to horizontal velocity.
            if (rb2dParent.velocity.x > 0f)
            {
                sr.flipX = true;
            }
            else if (rb2dParent.velocity.x < 0f)
            {
                sr.flipX = false;
            }    
        }
    }
}
