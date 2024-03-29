using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Pathfinding;
using System;
using UnityEngineInternal;

/* Reference: 
https://www.youtube.com/watch?v=jvtFUfJ6CP8
https://www.youtube.com/watch?v=sWqRfygpl4I
*/

public class EnemyAI : MonoBehaviour
{
    
    [Header("settings")]
    [SerializeField] private float activateDistance = 10f;
    
    [SerializeField] private float attackDamage = 10f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpStrength = 3f;
    [SerializeField] private float nextWaypointDist = 3f;
    [SerializeField] private float jumpRequirement = 0.5f;
    [SerializeField] private float groundRadius = 0.3f;

    
    [Header("info")]

    [SerializeField] private bool facingRight = true;
    [SerializeField] private bool grounded;
    [SerializeField] private bool following = true;
    int currentWaypoint = 0;
    bool endOfPath = false;


    [Header("Attachments")]

    [SerializeField] private Transform target;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    [SerializeField] private Animator anim;
    Path path;
    Seeker seeker;
    Rigidbody2D rb2D;
    Health enemy;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (seeker.IsDone())
        {
            seeker.StartPath(rb2D.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if(!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, ground);
        if (following && TargetInRange()) {
            anim.SetBool("IsWalking", true);
            FollowPath();
        }
        if (!TargetInRange() && rb2D.velocity.x != 0) {
            anim.SetBool("IsWalking", false);
            rb2D.velocity = new Vector2(0, rb2D.velocity.y);
        }
        if (rb2D.velocity.x > 0 && !facingRight) {
            facingRight = true;
            transform.Rotate(0f, 180f, 0f);
        }
        else if (rb2D.velocity.x < 0 && facingRight) {
            facingRight = false;
            transform.Rotate(0f, 180f, 0f);
        }

    }

    private void FollowPath() {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            endOfPath = true;
            return;
        }
        else 
        {
            endOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb2D.position).normalized;
        rb2D.velocity = new Vector2(moveSpeed * direction.x, rb2D.velocity.y);

        float distance = Vector2.Distance(rb2D.position, path.vectorPath[currentWaypoint]);

        if (direction.y > jumpRequirement && grounded)
        {
            rb2D.velocity = new Vector2(rb2D.velocity.x, jumpStrength);
        }

        if (distance < nextWaypointDist)
        {
            currentWaypoint++;
        }
    }
    private void OnDrawGizmos() {
        Gizmos.DrawWireSphere(groundCheck.position, groundRadius);
    }

    bool TargetInRange() {
        return Vector2.Distance(transform.position, target.transform.position) < activateDistance;
    }

    void OnTriggerEnter2D (Collider2D Hit) {
        enemy = Hit.GetComponent<Health>();
        if (enemy != null) {
            enemy.Hurt(attackDamage, 1);
        } 
    }
}