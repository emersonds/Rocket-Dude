using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    [SerializeField]
    private float angleCorrection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    private void FollowMouse()
    {
        //  Get mouse position in world space
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Get angle in radians
        float angleRad = Mathf.Atan2(mousePos.y - transform.position.y, mousePos.x - transform.position.x);

        // Convert angle to degrees, correct angle for rocket launcher angle
        float angleDeg = ((180 / Mathf.PI) * angleRad) - angleCorrection;

        // Rotate rocket launcher
        transform.rotation = Quaternion.Euler(0, 0, angleDeg);
    }
}
