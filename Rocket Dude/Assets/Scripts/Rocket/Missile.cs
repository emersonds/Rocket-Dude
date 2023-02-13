using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private LayerMask ammoLayer;

    private Collider2D[] inExplosionRadius = null;

    [SerializeField]
    private float explosiveForceMultiplier = 1000f;
    private float explosionRadius = 5f;

    /// <summary>
    /// Runs when the missile collides with something
    /// </summary>
    /// <param name="collision"></param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Explode();
    }

    /// <summary>
    /// Create an explosion when the object collides with something
    /// </summary>
    private void Explode()
    {
        inExplosionRadius = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D coll in inExplosionRadius)
        {
            Rigidbody2D collRb2d = coll.GetComponent<Rigidbody2D>();
            if (collRb2d != null)
            {
                // Get distance from center of explosion to object
                Vector2 distance = coll.transform.position - transform.position;

                // Adjust explosive force depending on distance.
                // Check greater than 0 to prevent NaN error (divide by 0)
                if (distance.magnitude > 0)
                {
                    float explosiveForce = explosiveForceMultiplier / distance.magnitude;
                    collRb2d.AddForce(distance.normalized * explosiveForce, ForceMode2D.Impulse);

                    Debug.Log($"Obj: {coll.name}\nMagn: {distance.magnitude}\nexplforce: {explosiveForce}\nAddForceForce: {distance.normalized * explosiveForce}");
                }
            }
        }

        // Destroy object after exploding
        Destroy(gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
