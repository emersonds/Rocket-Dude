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
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        rb2dParent = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
    }

    private void Flip()
    {
        if (rb2dParent != null)
        {
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
