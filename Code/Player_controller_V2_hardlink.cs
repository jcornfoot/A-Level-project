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

public enum PlayerState //Constants for player state
{
    IDLE,
    WALKING,
    JUMPING
}

public class Player_controller_V2 : MonoBehaviour
{
    //Initialisation settings
    public PlayerState PState = PlayerState.IDLE; //Set state to idle

    [Header("Settings")]
    public float fSpeed = 6.0f;
    public float fJumpStrength = 12.0f;
    public float fGroundRadius;

    [Header("Diagnostics")] //Using these to debug
    public bool bGrounded;
    public bool bFacingRight = true;
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

    
    //Runs each frame
    void Update()
    {
       //Check current inputs and player state so the state machine can work
       CheckInput();
       CheckMoveDirection();
       AnimationUpdate();
       CheckIfCanJump();

       //Start of the state machine, figures out and sets the players curent state and sets various values or calls functions depending on that state
       if (PState == PlayerState.IDLE)
       {
            if (fMoveDirection != 0) //fMoveDirection use GetAxisRaw which returns either: -1(left), 0(no input) or 1(right)
            {
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
                PState = PlayerState.JUMPING;
                Jump();
            }
       }
       else if (PState == PlayerState.WALKING)
       {
            if (Input.GetButtonDown("Jump") && bCanJump)
            {
                PState = PlayerState.JUMPING;
                Jump();
            }
            else if (fMoveDirection == 0)
            {
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
       //Using a state machine here so I can isolate the movement from the input somewhat
    }

    //Called every tick of the physics engine ~50 ticks/second, better to add the movement functions here and have the input stuff in update so inputs aren't missed
    private void FixedUpdate()
    {
        ApplyMovement();
        CheckGrounded();
    }

    private void CheckInput()
    {
        fMoveDirection = Input.GetAxisRaw("Horizontal");
    }

    //Doesn't need to be in FixedUpdate as its only adding to the players vertical velocity, impulse rather than continuous like the right/left movement
    private void Jump()
    {
        if (bCanJump) //Probably unnecessary as it is already checked before this function is called anyway
        {
            RB2D.velocity = new Vector2(RB2D.velocity.x, fJumpStrength); //Takes an x and y velocity and sets the rigid body's velocity to that, velocity.x == current velocity of the rigidbody
        }
    }

    private void ApplyMovement()
    {
        RB2D.velocity = new Vector2(fSpeed * fMoveDirection, RB2D.velocity.y); //Same as above but acting on the x instead of the y
    }

    //Creates a circle around the base of the player that sets the bGrounded bool when it overlaps with the ground
    private void CheckGrounded()
    {
        bGrounded = Physics2D.OverlapCircle(GroundCheck.position, fGroundRadius, IsGround);
    }

    //Flips the player model depending on the movement direction
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


    //Used to set a boolean in the animation controller that changes to and from the walking animation
    private void AnimationUpdate()
    {
        Anim.SetBool("IsWalking", bIsWalking);
    }

    //Draws a sphere over the grouncheck sphere for debug purposes
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(GroundCheck.position, fGroundRadius);
    }

}
