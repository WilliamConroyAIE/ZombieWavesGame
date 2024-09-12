using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement1 : MonoBehaviour
{
    public CharacterController controller;
    public float speed = 12f;
    public float gravity = -19.32f;
    Vector3 velocity;
    public Transform gCheck;
    public float gDistance = 0.4f;
    public LayerMask gMask;
    bool isGrounded;

    void Update()
    {
        isGrounded = Physics.CheckSphere(gCheck.position, gDistance, gMask);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        
        controller.Move(move * speed * Time.deltaTime);
        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
    }
}
