using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // movement setting
    public float moveForce = 10f;
    public float jumpoForce = 5f;
    public float airDrag = 0.5f;

    // Swinging setting 
    public float swingForce = 10f;
    public float maxSwingDistance = 10f;
    public transform hookPoint;
    public LineRenderer ropeLine;


    // other variables
    Rigidbody rb;
    bool isGrounded = false;
    bool isSwinging = false;
    Vector3 swingAnchor; 

    // Initializes the player's Rigidbody and LineRenderer
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.drag = airDrag; // Physics E: Air Resistance
    }

    // Update movement, jump and swinging logic
    void Update() 
    {
        

        
    }
    // WASD 
    void HandleMovement() 
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        Vector3 move = new Vector3(moveX, 0 ,moveZ) * moveForce;
        rb.AddForce(move, ForceMode.Force); // Physics C: Based Movement 
    }
    // swing logic
    void HandleSwinging()
    {
        // Calculate direction towards the swing anchor point
        Vector3 direction = (swingAnchor - transform.position).normalized;

        // Apply swinging force towards the anchor
        rb.AddForce(direction * swingForce, ForceMode.Force);

        // Update rope visual to show the swinging motion (LineRenderer)
        ropeLine.SetPosition(0, hookPoint.position);
        ropeLine.SetPosition(1, transform.position);
    }
    
    void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Attempt to hook to a valid surface
    void TryToHook()
    {
        RaycastHit hit; 
        
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxSwingDistance))
        {
            if (hit.collider.CompareTag("Swingable"))  
            {
                swingAnchor = hit.point;  // Set the hook point
                isSwinging = true;  // Start swinging
                ropeLine.enabled = true;  // Enable rope visual effect
                AttachSwing(hit);  // Attach spring joint to start physics-based swinging
            }
        }
    }

    // Attach a spring joint to simulate the swinging physics
    void AttachSwing(RaycastHit hit)
    {
        // Don't Forget to add a SpringJoint component for simulating the swing
        SpringJoint spring = gameObject.AddComponent<SpringJoint>();
        spring.connectedBody = hit.rigidbody;  // Connect spring to hit surface's Rigidbody
        spring.spring = 10f;  // Control strength of spring (affects swing speed)
        spring.damper = 1f;  // Damping to smooth out the swinging motion
        spring.maxDistance = maxSwingDistance;  // Limit how far the player can swing
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
