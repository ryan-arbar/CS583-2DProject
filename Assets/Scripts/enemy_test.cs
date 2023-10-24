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
    public float turnSpeed = 120.0f;
    public float detectionDistance = 1.0f;
    public LayerMask obstacleLayer;

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
            if (IsObstacleInFront())
            {
                TurnToAvoidObstacle();
            }
            else
            {
                Patrol();
            }
        }
        else
        {
            // Borb is dashing
            if (Time.time - dashStartTime >= dashDuration)
            {
                isDashing = false;
                SetRandomPatrolDestination();
            }
        }
    }

    private void SetRandomPatrolDestination()
    {
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
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        return Physics.Raycast(transform.position, forward, detectionDistance, obstacleLayer);
    }

    private void TurnToAvoidObstacle()
    {
        transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
    }

    private void Patrol()
    {
        if (!agent.hasPath || agent.remainingDistance < 0.1f)
        {
            SetRandomPatrolDestination();
        }
    }

    public void Dash()
    {
        agent.speed = dashSpeed;
        agent.SetDestination(transform.position + transform.forward * detectionDistance);
        isDashing = true;
        dashStartTime = Time.time;
    }
}
