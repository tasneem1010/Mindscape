using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Movement speed
    public float footstepInterval = 0.5f; // Time between footsteps in seconds

    private Rigidbody rb;
    public List<AudioSource> footsteps;
    private int footstepIndex = 0;
    private float footstepTimer = 0f;

    private FixedJoystick moveJoystick;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Prevent Rigidbody rotation from physics
        var joystick = GameObject.Find("Move Joystick");
        if (joystick != null)
            moveJoystick = joystick.GetComponent<FixedJoystick>();
    }

    void Update()
    {
        // Handle input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if(moveJoystick != null && moveJoystick.gameObject.activeInHierarchy)
        {
            moveX =moveJoystick.Horizontal;
            moveZ = moveJoystick.Vertical; 
        }

        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        Vector3 velocity = move * speed;
        rb.velocity = new Vector3(velocity.x, rb.velocity.y, velocity.z);

        if (footsteps.Count > 0 && velocity.magnitude > 0f)
        {
            footstepTimer += Time.deltaTime;

            if (footstepTimer >= footstepInterval && footsteps.Count > 0)
            {
                footsteps[footstepIndex].Play();
                footstepIndex = (footstepIndex + 1) % footsteps.Count;
                footstepTimer = 0f; 
            }
        }
    }
}
