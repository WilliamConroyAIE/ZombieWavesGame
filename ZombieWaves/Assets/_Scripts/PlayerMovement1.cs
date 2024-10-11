using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    [Header("StandardVariables")]
    public CharacterController controller;
    public float speed, walkSpeed = 8f, sprintSpeed = 12f, gravity = -9.81f, jumpHeight = 3f, groundDistance = 0.4f, sprintMultiplier = 0f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public bool isSprinting;
    Vector3 velocity;
    bool isGrounded;

    
    [Header("FootStepSounds")]
    public AudioClip footStepSound;
    public float footStepDelay;
    private float nextFootstep = 0;

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y <0)
        {
            velocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 motion = transform.right * x + transform.forward * z;
        controller.Move(motion * speed * Time.deltaTime);

        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) && isGrounded)
        {
            nextFootstep -= Time.deltaTime;
            if (nextFootstep <= 0) 
               {
                GetComponent<AudioSource>().PlayOneShot(footStepSound, 0.7f);
                nextFootstep += footStepDelay;
               }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.W) )
        {
            isSprinting = true;
            sprintMultiplier += Time.deltaTime;

            if (sprintMultiplier > sprintSpeed)
            {
                sprintMultiplier = sprintSpeed;
            }
        }
        else
        {
            speed = walkSpeed;
            isSprinting = false;
        }
    }
}


