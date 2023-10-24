using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class sporb_script : MonoBehaviour
{
    public float speed = 1f;
    public float turnSpeed = 5f; // Adjust as needed
    public float detectionRadius = 1f;
    public float patrolRadius = 3f;

    public Transform target;
    Path path;
    Seeker seeker;
    Rigidbody2D rb;
    int currentWaypoint = 0;
    public float nextWaypointDistance;
    Vector2 initialPosition;

    public float pathRefreshRate = .3f; // Time interval between path updates

    private void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initialPosition = rb.position;

        UpdatePath();
        InvokeRepeating("UpdatePath", 0, pathRefreshRate);
    }

    void UpdatePath()
    {
        if (Vector2.Distance(target.position, rb.position) <= detectionRadius)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
        else
        {
            Vector2 randomPoint = initialPosition + Random.insideUnitCircle * patrolRadius;
            seeker.StartPath(rb.position, randomPoint, OnPathComplete);
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
            currentWaypoint = 0;
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

    private void FixedUpdate()
    {
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count)
        {
            return; // We've reached the end of the path
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
   
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.DrawWireSphere(transform.position, patrolRadius);
    }
}