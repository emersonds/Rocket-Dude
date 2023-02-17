using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketController : MonoBehaviour
{
    private const float ANGLE_CORRECTION = 135f;

    private const float ANGLE_CORRECTION_FLIPPED = 90f;

    private bool flipped;

    private bool canShoot = true;

    private Vector2 mousePos;

    private float angleRad;

    private float angleDeg;

    private WaitForSeconds shootCooldownTime = new WaitForSeconds(0.4f);

    private Vector2 shoulderPoint;
    private Vector2 shoulderPointFlipped;

    private Vector2 firePointPosition;
    private Vector2 firePointPositionFlipped;

    private SpriteRenderer sr;

    [SerializeField]
    private GameObject missile;

    [SerializeField]
    private Transform firePoint;

    [SerializeField]
    private float fireForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        // Assign component reference
        sr = GetComponent<SpriteRenderer>();

        // Get shoulder point positions
        shoulderPoint = transform.localPosition;
        shoulderPointFlipped = new Vector2(-transform.localPosition.x, transform.localPosition.y);

        // Get fire point positions;
        firePointPosition = firePoint.localPosition;
        firePointPositionFlipped = new Vector2(-firePoint.localPosition.x, firePoint.localPosition.y);
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
    }

    /// <summary>
    /// Rotates and flips the rocket based on mouse position.
    /// </summary>
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
            firePoint.localPosition = firePointPositionFlipped;
        }
        else
        {
            transform.localPosition = shoulderPoint;
            firePoint.localPosition = firePointPosition;
        }
    }

    public void Shoot()
    {
        if (canShoot)
        {
            GameObject tempMissile = Instantiate(missile, firePoint.position, Quaternion.identity);

            Rigidbody2D missileRb2d = tempMissile.GetComponent<Rigidbody2D>();

            Vector2 aimDir = mousePos - missileRb2d.position;
            float aimAngle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg - 90f;

            missileRb2d.rotation = aimAngle;
            missileRb2d.AddForce(aimDir.normalized * fireForce, ForceMode2D.Impulse);

            StartCoroutine(ShootCooldown());
        }
    }

    private IEnumerator ShootCooldown()
    {
        canShoot = false;
        yield return shootCooldownTime;
        canShoot = true;
    }
}
