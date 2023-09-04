using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
Source:
- https://www.youtube.com/playlist?list=PLy78FINcVmjA0zDBhLuLNL1Jo6xNMMq-W (Video Series)

*/

public class Player_controller_test : MonoBehaviour
{
    
    private float MovementInputDirection;

    private bool IsFacingRight = true;
    private bool IsWalking;
    private bool IsGrounded;
    private bool CanJump;

    private Rigidbody2D rb;
    private Animator anim;

    public float MovementSpeed = 5.0f;
    public float JumpForce = 12.0f;
    public float GroundCheckRadius;

    public Transform GroundCheck;

    public LayerMask WhatIsGround;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    
    void Update()
    {
        CheckInput();
        CheckMovementDirection();
        UpdateAnimations();
        CheckIfCanJump();
    }

    private void FixedUpdate() 
    {
        ApplyMovement();
        CheckGrounded();
    }


    private void CheckInput()
    {
        MovementInputDirection=Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump"))
        {
            Jump();
        }
    }

    private void ApplyMovement()
    {
        rb.velocity = new Vector2(MovementSpeed * MovementInputDirection, rb.velocity.y);
    }

    private void CheckMovementDirection ()
    {
        if (IsFacingRight && MovementInputDirection < 0)
        {
            Flip();
        }
        else if (!IsFacingRight && MovementInputDirection > 0)
        {
            Flip();
        }

        if (rb.velocity.x != 0)
        {
            IsWalking = true;
        }
        else
        {
            IsWalking = false;
        }
    }

    private void CheckGrounded()
    {
        IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundCheckRadius, WhatIsGround);
    }

    private void Flip()
    {
        IsFacingRight = !IsFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void Jump()
    {
        if (CanJump)
        {
            rb.velocity = new Vector2(rb.velocity.x, JumpForce);
        }
    }

    private void UpdateAnimations()
    {
        anim.SetBool("IsWalking", IsWalking);
    }

    private void CheckIfCanJump()
    {
        if (IsGrounded && rb.velocity.y <=0)
        {
            CanJump = true;
        }
        else
        {
            CanJump = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, GroundCheckRadius);
    }
}
