using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Based on:
|State machine
|\Source: https://www.noveltech.dev/reusable-character-controller-platformer/
\Source: https://github.com/Wally869/TutorialPlatformer2D/blob/master/Assets/Scripts/PlayerController.cs

|Movement
\https://www.youtube.com/playlist?list=PLy78FINcVmjA0zDBhLuLNL1Jo6xNMMq-W (Video Series)
*/

public enum PlayerState
{
    IDLE,
    WALKING,
    JUMPING
}

public class Player_controller_V2 : MonoBehaviour
{
    public PlayerState PState = PlayerState.IDLE;

    [Header("Settings")]
    public float fSpeed = 6.0f;
    public float fJumpStrength = 12.0f;
    public float fGroundRadius;

    [Header("Diagnostics")]
    public bool bGrounded;
    public bool bFacingRight = true;
    //public bool bStateChanged = false;
    public bool bCanJump;
    public bool bIsWalking;

    private float fMoveDirection;

    private Rigidbody2D RB2D;
    private Animator Anim;

    public Transform GroundCheck;
    public LayerMask IsGround;

    void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
    }

    
    void Update()
    {
       //bStateChanged = false;

       CheckInput();
       CheckMoveDirection();
       AnimationUpdate();
       CheckIfCanJump();

       if (PState == PlayerState.IDLE)
       {
            if (fMoveDirection != 0)
            {
                //bStateChanged = true;
                PState = PlayerState.WALKING;
                if (fMoveDirection > 0)
                {
                    bFacingRight = true;
                }
                else 
                {
                    bFacingRight = false;
                }
            }
            else if (Input.GetButtonDown("Jump") && bCanJump)
            {
                //bStateChanged = true;
                PState = PlayerState.JUMPING;
                Jump();
            }
       }
       else if (PState == PlayerState.WALKING)
       {
            if (Input.GetButtonDown("Jump") && bCanJump)
            {
                //bStateChanged = true;
                PState = PlayerState.JUMPING;
                Jump();
            }
            else if (fMoveDirection == 0)
            {
                //bStateChanged = true;
                PState = PlayerState.IDLE;
            }
       }
       else if (PState == PlayerState.JUMPING)
       {
            if (bCanJump == true)
            {
                if (fMoveDirection != 0)
                {
                    PState = PlayerState.WALKING;
                }
                else
                {
                    PState = PlayerState.IDLE;
                }
            }
       }
    }

    private void FixedUpdate()
    {
        ApplyMovement();
        CheckGrounded();
    }

    private void CheckInput()
    {
        fMoveDirection = Input.GetAxisRaw("Horizontal");
    }

    private void Jump()
    {
        if (bCanJump)
        {
            RB2D.velocity = new Vector2(RB2D.velocity.x, fJumpStrength);
        }
    }

    private void ApplyMovement()
    {
        RB2D.velocity = new Vector2(fSpeed * fMoveDirection, RB2D.velocity.y);
    }

    private void CheckGrounded()
    {
        bGrounded = Physics2D.OverlapCircle(GroundCheck.position, fGroundRadius, IsGround);
    }

    private void Flip() 
    {
        bFacingRight = !bFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    private void CheckMoveDirection()
    {
        if (bFacingRight && fMoveDirection < 0)
        {
            Flip();
        }
        else if (!bFacingRight && fMoveDirection > 0)
        {
            Flip();
        }

        if (fMoveDirection != 0)
        {
            bIsWalking = true;
        }
        else
        {
            bIsWalking = false;
        }
    }

    private void CheckIfCanJump()
    {
        if (bGrounded)
        {
            bCanJump = true;
        }
        else
        {
            bCanJump = false;
            PState = PlayerState.JUMPING;
        }
    }


    private void AnimationUpdate()
    {
        Anim.SetBool("IsWalking", bIsWalking);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, fGroundRadius);
    }

}
