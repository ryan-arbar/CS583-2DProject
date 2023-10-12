using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class enemy_test : MonoBehaviour
{

    public float patrolRadius = 5.0f;
    public float patrolSpeed = 3.0f;
    public float dashSpeed = 10.0f;
    public float dashDuration = 0.5f;
    public float turnSpeed = 120.0f; // Adjust this value for turning speed.
    public float detectionDistance = 1.0f;
    public LayerMask obstacleLayer; // Set this in the Inspector to the layer containing obstacles.

    private Vector3 targetPosition;
    private bool isDashing;
    private float dashStartTime;


    [SerializeField] Transform target;

    NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        SetRandomPatrolDestination();
    }

    private void Update() 
    {

        if (!isDashing)
        {
            // Check if there's an obstacle in front of Borb.
            if (IsObstacleInFront())
            {
                TurnToAvoidObstacle();
            }
            else
            {
                // Continue patrolling.
                Patrol();
            }
        }
        else
        {
            // Borb is dashing.
            if (Time.time - dashStartTime >= dashDuration)
            {
                // Dash completed, stop dashing and continue patrolling.
                isDashing = false;
                SetRandomPatrolDestination();
            }
        }
    }

    private void SetRandomPatrolDestination()
    {
        // Set a random destination within the patrol radius.
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, -1);
        targetPosition = hit.position;

        agent.speed = patrolSpeed;
        agent.SetDestination(targetPosition);
    }

    private bool IsObstacleInFront()
    {
        // Check if there's an obstacle in the direction Borb is facing.
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        return Physics.Raycast(transform.position, forward, detectionDistance, obstacleLayer);
    }

    private void TurnToAvoidObstacle()
    {
        // Turn Borb to avoid obstacles.
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
    }

    private void Patrol()
    {
        // Continue patrolling.
        if (!agent.hasPath || agent.remainingDistance < 0.1f)
        {
            SetRandomPatrolDestination();
        }
    }

    public void Dash()
    {
        // Perform a dash.
        agent.speed = dashSpeed;
        agent.SetDestination(transform.position + transform.forward * detectionDistance);
        isDashing = true;
        dashStartTime = Time.time;
    }
}
