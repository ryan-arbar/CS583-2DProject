using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class borb_script : MonoBehaviour
{

    public float speed = 1f;
    public float turnSpeed = 5f; // Adjust as needed
    public float detectionRadius = 1f;


    public Transform target;
    public float minimumDistance;
    public float nextWaypointDistance = 1.5f;

    public float dashForce = 1f; // Force for the dash movement
    public float dashCooldown = 2f; // Time interval between dashes

    public float pathRefreshRate = 1f; // Time interval between path updates

    Path path;

    private float nextDashTime;
    Seeker seeker;
    Rigidbody2D rb;


    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;

        UpdatePath();
        InvokeRepeating("UpdatePath", 0, pathRefreshRate);
    }

    void UpdatePath()
    {
        if (Vector2.Distance(target.position, rb.position) <= detectionRadius)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (path != null)
        {
            path.Release(this); // Release the old path back to the pool
        }

        if (!p.error)
        {
            path = p;
            path.Claim(this); // Claim the new path
        }
    }

    private void OnDestroy()
    {
        if (path != null)
        {
            path.Release(this);
        }
    }

    private void OnDisable()
    {
        if (path != null)
        {
            path.Release(this);
        }
    }

    void FacePlayer()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90; // Adjust based on your sprite's orientation
        Quaternion targetRotation = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * turnSpeed);
    }


    void FixedUpdate()
    {

        float distanceToPlayer = Vector2.Distance(transform.position, target.position);

        if (path == null)
        {
            return;
        }


        if (distanceToPlayer <= detectionRadius) 
        {
            FacePlayer();

            // If ready to dash, apply a force in the direction the enemy is facing
            if (Time.time > nextDashTime)
            {
                    rb.AddForce(transform.up * dashForce, ForceMode2D.Impulse);
                    nextDashTime = Time.time + dashCooldown;
            }
        }
        else
        {
            //ATTACK CODE
        }

        
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
