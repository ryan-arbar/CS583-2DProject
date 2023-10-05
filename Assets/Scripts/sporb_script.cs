using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sporb_script : MonoBehaviour
{
    public float roamRadius = 10f;
    public float followRadius = 5f;
    public float roamSpeed = 2f;
    public float followSpeed = 4f;

    private Transform player;
    private UnityEngine.AI.NavMeshAgent agent;
    private Vector3 roamPosition;
    private float nextActionTime = 0f;
    private float timeBetweenActions = 3f;

    private void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform; // Adjust the tag as needed.

        // Start with a random roam position.
        roamPosition = GetRandomRoamPosition();
        agent.speed = roamSpeed;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= followRadius)
        {
            // Player is within follow range, chase the player.
            agent.SetDestination(player.position);
            agent.speed = followSpeed;

            // Keep the enemy's orientation stable.
            Vector3 lookAtPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
            transform.LookAt(lookAtPosition);
        }
        else if (Time.time > nextActionTime)
        {
            // Player is outside follow range, or it's time for a new action.
            if (distanceToPlayer > followRadius * 2)
            {
                // If player is far away, set a new roam position.
                roamPosition = GetRandomRoamPosition();
                agent.speed = roamSpeed;
            }

            // Move towards the current roam position.
            agent.SetDestination(roamPosition);

            // Keep the enemy's orientation stable.
            Vector3 lookAtPosition = new Vector3(roamPosition.x, transform.position.y, roamPosition.z);
            transform.LookAt(lookAtPosition);

            // Schedule the next action.
            nextActionTime = Time.time + timeBetweenActions;
        }
    }

    private Vector3 GetRandomRoamPosition()
    {
        // Generate a random point within the roam radius.
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
        return hit.position;
    }
}