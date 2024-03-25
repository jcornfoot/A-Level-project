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

/* 
BUGS:
- Spamming jump causes a double jump due to coyote time and ground detect circle reporting grounded while in air 
    - If shrunk or removed, need to implement jump buffering to replace functionality
*/


public class PlayerControllerV3 : MonoBehaviour
{
    [Header("State")]
    private bool sprintTimer;
    [SerializeField] private bool walking;
    [SerializeField] private bool aiming;
    [SerializeField] private bool sprinting;
    [SerializeField] private bool lookingRight = true;
    [SerializeField] private bool grounded;
    [SerializeField] private bool canMove;
    [SerializeField] private bool canJump;
    [SerializeField] private bool canAim;
    [SerializeField] private bool canSprint;
    [SerializeField] private bool coyoteTime;
    [SerializeField] private float coyoteTimer;
    [SerializeField] private float airTimer;
    
    [SerializeField] private float moveDirection;
    [SerializeField] private float sprintDelay = 0.2f;
    [SerializeField] private float sprintTime;
    [SerializeField] private int tapAmount;

    [Header("Config")]
    [SerializeField] private float walkSpeed = 6.0f;
    [SerializeField] private float sprintSpeed = 9.0f;
    [SerializeField] private float jumpForce = 12.0f;
    [SerializeField] private float groundRadius = 0.20f;
    [Header("Attachments")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    [SerializeField] private GameObject wManager;
    [SerializeField] private Rigidbody2D RB2D;
    [SerializeField] private Animator Anim;
    [SerializeField] private WeaponManager weaponManager;
    
    void Start()
    {
        UnityEngine.Rendering.DebugManager.instance.enableRuntimeUI = false; /* Disables the debug canvas, preventing a freeze */
        RB2D = gameObject.GetComponent<Rigidbody2D>();
        Anim = gameObject.GetComponent<Animator>();
        weaponManager = wManager.GetComponent<WeaponManager>();
    }

    void Update()
    {
        SetState();
        AirControl();
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
        CheckJumpable();
        CheckCoyoteTime();
    }


/* Checks */
    private void CheckGrounded() {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, ground);
    }

    private void CheckAimable() {
        if (grounded && !sprinting) canAim = true;
        else canAim = false;
    }

    private void CheckMovable() {
        canMove = true;
    }

    private void CheckSprintable() {
        if (grounded && walking && !aiming) canSprint = true;
        else canSprint = false;
    }

    private void CheckJumpable() {
        if (grounded || coyoteTime) {
            canJump = true;
        }
        else {
            canJump = false;
        }
    }

    private void CheckCoyoteTime() {
        /* reference: https://www.youtube.com/watch?v=RFix_Kg2Di0 */
        if (grounded) {
            airTimer = 0;
        }
        else {
            airTimer += Time.deltaTime;
        }
        if (!grounded && airTimer < coyoteTimer) {
            coyoteTime = true;
        }
        else {
            coyoteTime = false;
        }
    }


/* Transitions */

    private void AirControl() {
        if (Input.GetKeyDown("space")&& canJump) {
            Jump();
        }
        if (Input.GetKeyUp("space") && RB2D.velocity.y > 0) {
            RB2D.velocity = new Vector2(RB2D.velocity.x, RB2D.velocity.y * 0.5f);
        }
        if (RB2D.velocity.y < 0) RB2D.gravityScale = 2.5f;
        else RB2D.gravityScale = 2;
    }
    private void MoveControl() {
        moveDirection = Input.GetAxisRaw("Horizontal");
        if (canSprint && !sprinting && !sprintTimer) {
            sprintTimer = true;
            sprintTime = sprintDelay;
            StartCoroutine(Sprint(moveDirection));
        }
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
            weaponManager.currentWeapon.SetActive(false);
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
        weaponManager.currentWeapon.SetActive(true);
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

    void OnDisable() {
        Time.timeScale = 0;
    }
}