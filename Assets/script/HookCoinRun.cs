using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCoinRun : BaseCharacter
{
    public float detectRange = 7f;
    public float escapeSpeed = 2f;
    public float chaseSpeedMultiplier = 2f;
    public float veryCloseRange = 3f;
    public float zigzagFrequency = 3f;
    public float zigzagAmplitude = 2f;
    public float jumpInterval = 2f;

    private Transform player;
    private float jumpTimer;

    protected override void Awake()
    {
        base.Awake(); 
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    void FixedUpdate()
    {
        if (!player) return;

        float distance = Vector3.Distance(transform.position, player.position);
        if (distance < detectRange)
        {
            Vector3 flatDir = (transform.position - player.position);
            flatDir.y = 0;
            flatDir.Normalize();

            Vector3 zigzag = Vector3.Cross(flatDir, Vector3.up) * Mathf.Sin(Time.time * zigzagFrequency) * zigzagAmplitude;
            Vector3 finalDir = (flatDir + zigzag).normalized;

            float speed = (distance < veryCloseRange) ? escapeSpeed * chaseSpeedMultiplier : escapeSpeed;
            Vector3 velocity = finalDir * speed;
            velocity.y = rb.velocity.y; // keep Y axis to make the coin move only XZ

            rb.velocity = velocity;

            // Jump
            jumpTimer += Time.fixedDeltaTime;
            if (jumpTimer >= jumpInterval && IsGrounded())
            {
                Jump(); // call form character
                jumpTimer = 0f;
            }
        }
    }
}
