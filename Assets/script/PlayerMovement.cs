using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseCharacter
{
    // movement setting
    
    public float airDrag = 0.5f;

    // Swinging setting 
    public float swingForce = 10f;
    public float maxSwingDistance = 30f;
    public LineRenderer ropeLine;

    // other variables
    
    bool isGrounded = false;
    bool isSwinging = false;
    Vector3 swingAnchor; 

    [SerializeField] Transform cameraTransform;
    
    // awake gonna get in animator on squid to work but after the ui and end credit is finished
    protected override void Awake()
    {
        base.Awake(); 
    }

    // Initializes the player's Rigidbody and LineRenderer
    void Start()
    {
        
        rb.drag = airDrag; // Physics E: Air Resistance

        if (ropeLine != null)
        {
            ropeLine.positionCount = 2;      // Must set to 2 to avoid index error
            ropeLine.enabled = false;        // Optional: hide rope until swinging
        }
    }

    // Update movement, jump and swinging logic
    void Update() 
    {
        if (!isSwinging) 
        {
            HandleMovement();
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)  
        {
            Jump();
        }

        if (isSwinging)  // If swinging, apply swinging force
        {
            HandleSwinging();
        }

        // Handle hook attempt when player clicks and remove in one click
        if (Input.GetMouseButtonDown(0))
        {
            if (isSwinging)
            {
                ReleaseHook(); 
            }
            else
            {
                TryToHook(); 
            }
        }

        
    }
    // WASD 
    void HandleMovement() 
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // made new camera to make gameplay more impressed
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 move = (camForward * moveZ + camRight * moveX).normalized;
        rb.AddForce(move.normalized* moveForce, ForceMode.Force); // Physics C: Based Movement 
        // make model facing the right direction i find how to made this in reddit thank for random guy that name deleted account 
        if (move.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(move, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }
    // swing logic
    void HandleSwinging()
    {
        // Calculate direction towards the swing anchor point
        Vector3 direction = (swingAnchor - transform.position).normalized;

        // Apply swinging force towards the anchor
        rb.AddForce(direction * swingForce, ForceMode.Force);

        // Update rope visual to show the swinging motion (LineRenderer)
        if (ropeLine != null)
        {
            ropeLine.SetPosition(0, swingAnchor);
            ropeLine.SetPosition(1, transform.position);
        }
    }
    
    new void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // Attempt to hook to a valid surface
    void TryToHook()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit; 
        
        if (Physics.Raycast(ray, out hit, maxSwingDistance))
        {

            // added the coin check hook if not swingable not gonna swinging
            if (hit.collider.CompareTag("Coin"))
            {
                Coin coin = hit.collider.GetComponent<Coin>();
                if (coin != null)
                {
                    coin.CollectByHook(); // Only for hookable coins
                }
            }

            if (hit.collider.CompareTag("Swingable"))  
            {
                swingAnchor = hit.point;  // Set the hook point
                isSwinging = true;  // Start swinging
                if (ropeLine != null)
                {
                ropeLine.enabled = true; // Enable rope visual effect
                }  
                
                AttachSwing(hit);  // Attach spring joint to start physics-based swinging
            }
            //else will be like robe fall like cant reach
            
            
        }
    }

    // Attach a spring joint to simulate the swinging physics
    void AttachSwing(RaycastHit hit)
    {
        // Don't Forget to add a SpringJoint component for simulating the swing
        SpringJoint spring = gameObject.AddComponent<SpringJoint>();
        spring.autoConfigureConnectedAnchor = false;
        spring.connectedBody = hit.rigidbody;  // Connect spring to hit surface's Rigidbody
        
        spring.spring = 10f;  // Control strength of spring (affects swing speed)
        spring.damper = 1f;  // Damping to smooth out the swinging motion
        spring.maxDistance = maxSwingDistance;  // Limit how far the player can swing
    }

        void ReleaseHook()
    {
        isSwinging = false;

        SpringJoint sj = GetComponent<SpringJoint>();
        if (sj != null)
        {
            Destroy(sj);
        }

        if (ropeLine != null)
        {
            ropeLine.enabled = false;
        }
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
