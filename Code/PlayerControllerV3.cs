using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEditor.Rendering.BuiltIn.ShaderGraph;
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
    public bool sprintTimer;
    public bool walking;
    public bool aiming;
    public bool sprinting;
    public bool lookingRight = true;
    public bool grounded;
    public bool canMove;
    public bool canJump;
    public bool canAim;
    public bool canSprint;
    
    public float moveDirection;
    public float sprintDelay = 0.2f;
    public float sprintTime;
    public int tapAmount;

    [Header("Config")]
    public float walkSpeed = 6.0f;
    public float sprintSpeed = 9.0f;
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
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false; /* Disables the debug canvas, preventing a freeze */
        Time.timeScale = 0.5f;
        RB2D = gameObject.GetComponent<Rigidbody2D>();
        Anim = gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        SetState();
        if (canMove) {
            MoveControl();
        }
        AimControl();
        
        UpdateAnimation();
    }

    private void SetState() {
        CheckGrounded();
        CheckAimable();
        CheckMovable();
        CheckSprintable();
        
        if (grounded) {
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
        canMove = true;
    }

    private void CheckSprintable() {
        if (grounded && walking && !aiming) canSprint = true;
        else canSprint = false;
    }


/* Transitions */
    private void MoveControl() {
        moveDirection = Input.GetAxisRaw("Horizontal");
        if (canSprint && !sprinting && !sprintTimer) {
            sprintTimer = true;
            sprintTime = sprintDelay;
            StartCoroutine(Sprint(moveDirection));
        }


        if (Input.GetKeyDown("space")&& canJump) Jump();
        if (moveDirection != 0) {
            walking = true;
        }
        else {
            walking = false;
            sprinting = false;
            sprintTimer = false;
        }
        Move();
    }

    private IEnumerator Sprint(float direction) {
        while (sprintTime > 0) {
            if ((Input.GetKeyDown("a") || Input.GetKeyDown("d")) && (direction == moveDirection)) {
            sprinting = true;
            sprintTimer = false;
            yield break;
            }
            sprintTime -= Time.deltaTime;
            yield return null;
        }
        sprinting = false;
    }
    private void AimControl() {
        if (Input.GetButton("Fire2") && canAim) {
            aiming = true;
            Aim();
        }
        else if (Input.GetButtonUp("Fire2") || !canAim && aiming) {
            WeaponPoint.SetActive(false);
            aiming = false;
        }
    }


/* States */
    private void Move() {
        UpdateOrientation();
        if (sprinting) RB2D.velocity = new Vector2(sprintSpeed * moveDirection, RB2D.velocity.y);
        else RB2D.velocity = new Vector2(walkSpeed * moveDirection, RB2D.velocity.y);
    }

    private void Jump() {
        RB2D.velocity = new Vector2(RB2D.velocity.x, jumpForce);
    }

    private void Aim() {
        WeaponPoint.SetActive(true);
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
    private void UpdateAnimation() {
        //jumping is a trigger
        //aiming is a bool
        //Idle is default

        Anim.SetBool("IsWalking", walking);
    }

    private void Flip() {
        lookingRight = !lookingRight;
        transform.Rotate(0f, 180f, 0f);
    }


    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }
}