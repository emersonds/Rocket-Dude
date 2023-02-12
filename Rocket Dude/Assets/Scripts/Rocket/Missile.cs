using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private LayerMask ammoLayer;

    private void Start()
    {

    }

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
        Debug.Log("Explode!");
        Destroy(gameObject);
    }
}
