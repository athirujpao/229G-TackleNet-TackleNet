using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public Transform target; // Player's Transform
    public Vector3 offset = new Vector3(0, 5, -10); // Default offset behind player
    public float smoothSpeed = 0.125f;  // Camera smoothness
    public float rotationSpeed = 5f;    // Mouse sensitivity

    private float yaw = 0f;
    private float pitch = 15f;

    void LateUpdate()
    {
        // Mouse input
        yaw += Input.GetAxis("Mouse X") * rotationSpeed;
        pitch -= Input.GetAxis("Mouse Y") * rotationSpeed;
        pitch = Mathf.Clamp(pitch, 10f, 45f); 

        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0);
        Vector3 desiredPosition = target.position + rotation * offset;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.LookAt(target);
    }
}
