using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Pathfinding;

/* Reference: 
https://www.youtube.com/watch?v=jvtFUfJ6CP8
https://www.youtube.com/watch?v=sWqRfygpl4I
*/

public class EnemyAI : MonoBehaviour
{
    public Transform target;

    public float moveSpeed = 10f;
    public float jumpStrength = 3f;
    public float nextWaypointDist = 3f;
    public float jumpRequirement = 0.5f;
    public float moveDirection;
    public float groundRadius = 0.3f;
    public bool grounded;
    public Transform groundCheck;
    public LayerMask ground;
    Path path;
    int currentWaypoint = 0;
    bool endOfPath = false;

    Seeker seeker;
    Rigidbody2D rb2D;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb2D = GetComponent<Rigidbody2D>();

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
}
