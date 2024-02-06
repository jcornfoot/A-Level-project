using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEditor.Tilemaps;
using UnityEngine;

/*
REFERENCES:
- PlayerController
- https://github.com/DanielDFY/Hollow-Knight-Imitation/blob/master/Hollow%20Knight/Assets/Scripts/Player/PlayerController.cs
*/


public class PlayerControllerV3 : MonoBehaviour
{
    [Header("State")]
    public bool lookingRight = true;

    public bool grounded;
    public bool canMove;
    public bool canJump;
    public bool canAim;
    public bool aiming;
    public float moveDirection;
    [Header("Config")]
    public float speed = 6.0f;
    public float jumpForce = 12.0f;
    public float groundRadius = 0.20f;
    [Header("Attachments")]
    public Transform groundCheck;
    public LayerMask ground;
    public GameObject WeaponPoint;



    private Rigidbody2D RB2D;
    private Animator Anim;
    
    void Start()
    {
        RB2D = gameObject.GetComponent<Rigidbody2D>();
        Anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        SetState();
        if (canMove) {
            Move();
            JumpControl();
        }
        AimControl();
        
        //UpdateAnimation();
    }

    private void SetState() {
        CheckGrounded();
        CheckAimable();
        CheckMovable();
        
        if (grounded && !aiming) {
            canJump = true;
        }
        else {
            canJump = false;
        }

        
    }


/* Checks */
    private void CheckGrounded() {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, ground);
    }

    private void CheckAimable() {
        if (grounded) canAim = true;
        else canAim = false;
    }

    private void CheckMovable() {
        if (aiming) canMove = false;
        else canMove = true;
    }

/* States */
    private void Move() {
        moveDirection = Input.GetAxisRaw("Horizontal");
        UpdateOrientation();

        RB2D.velocity = new Vector2(speed * moveDirection, RB2D.velocity.y);
    }

    private void Jump() {
        RB2D.velocity = new Vector2(RB2D.velocity.x, jumpForce);
    }

    private void Aim() {
        WeaponPoint.SetActive(true);
    }

/* Transitions */
    private void JumpControl() {
        if (Input.GetButtonDown("Jump") && canJump) Jump();
    }
    private void AimControl() {
        if (Input.GetButton("Fire2") && canAim) {
            if (Input.GetButtonDown("Fire2")) RB2D.velocity = new Vector2(0f, 0f); /* sets current x and y velocity to zero on first frame Fire2 is pressed, if not the player will slide when entering aiming while moving */
            //BUG: If first frame is in air, player slides when they contact the ground

            aiming = true;
            Aim();
        }
        else if (Input.GetButtonUp("Fire2")) { /* Disables the weapon point and it's children on first frame Fire2 is released */
            WeaponPoint.SetActive(false);
            aiming = false;
        }
    }

/* Appearance */

    private void UpdateOrientation() {
        if (lookingRight && moveDirection < 0) {
            Flip();
        }
        else if (!lookingRight && moveDirection > 0) {
            Flip();
        }

    }
    /*private void UpdateAnimation() {
        Anim.SetBool("IsWalking", isW)
    }*/

    private void Flip() {
        lookingRight = !lookingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}