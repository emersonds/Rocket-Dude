using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    #region Fields

    [SerializeField, Tooltip("How fast the player can move.")]
    private float moveSpeed = 1f;

    [SerializeField, Tooltip("How much force the player uses to jump.")]
    private float jumpForce = 0.3f;

    #endregion

    #region Properties

    public float MoveSpeed { get { return moveSpeed; } }

    public float JumpForce { get { return jumpForce; } }

    #endregion
}
