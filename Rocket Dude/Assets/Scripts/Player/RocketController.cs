using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private const float ANGLE_CORRECTION = 135f;

    private const float ANGLE_CORRECTION_FLIPPED = 90f;

    private bool flipped;

    private Vector2 mousePos;

    private float angleRad;

    private float angleDeg;

    private Vector2 shoulderPoint;
    private Vector2 shoulderPointFlipped;

    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        // Assign component reference
        sr = GetComponent<SpriteRenderer>();

        // Get shoulder point positions
        shoulderPoint = transform.localPosition;
        shoulderPointFlipped = new Vector2(-transform.localPosition.x, transform.localPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        //  Get mouse position in world space
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get angle in radians
        angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);

        // Convert angle to degrees, correct angle for rocket launcher angle
        angleDeg = ((180 / Mathf.PI) * angleRad) - ANGLE_CORRECTION;

        if (angleDeg < -60f && angleDeg > -220f)
        {
            angleDeg += ANGLE_CORRECTION_FLIPPED;
            sr.flipX = true;
            flipped = true;
        }
        else
        {
            sr.flipX = false;
            flipped = false;
        }

        // Rotate rocket launcher
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);

        // Move the rocket launcher to the correct shoulder
        if (flipped)
        {
            transform.localPosition = shoulderPointFlipped;
        }
        else
        {
            transform.localPosition = shoulderPoint;
        }
    }
}
