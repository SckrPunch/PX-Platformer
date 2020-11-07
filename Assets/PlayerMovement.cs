using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public float gravity;
    public Transform groundCheck;
    public Vector2 groundDistance;
    public LayerMask groundMask;
    public CharacterController controller;

    bool isGrounded;
    Vector2 gVelocity;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapBox(groundCheck.position, groundDistance, 0, groundMask);

        if(isGrounded && gVelocity.y < 0)
        {
            gVelocity.y = -2f;
        }

        float x = Input.GetAxis("Horizontal");

        Vector2 move = transform.right * x;

        controller.Move(move * speed * Time.deltaTime);
    }

}
