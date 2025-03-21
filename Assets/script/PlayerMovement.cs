using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float moveForce = 10f;
    public float jumpoForce = 5f;
    public float airDrag = 0.5f;
    Rigidbody rb;
    bool isGrounded = false;

    void Start()
    {
        rb.GetComponent<Rigidbody>();
        rb.drag = airDrag // Physics E: Air Resistance
    }

    Void Update() 
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0 ,moveZ) * moveForce;
        rb.AddForce(move, ForceMode.Force); // Physics C: Based Movement 

        if(Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpoForce, ForceMode.Impulse);
        }
    }
}
