using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public class BaseCharacter : MonoBehaviour
{
    public float moveForce = 10f;
    public float jumpForce = 5f;
    protected Rigidbody rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    protected void Jump()
    {
        if (IsGrounded())
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    protected bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.6f);
    }
}

