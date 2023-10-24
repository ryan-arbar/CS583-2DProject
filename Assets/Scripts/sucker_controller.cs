using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sucker_controller : MonoBehaviour
{
    public Transform player;
    public Transform anchor;
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 10.0f;
    public float vicinityRadius = 5.0f;
    public float inertiaDuration = 1.0f;

    private Rigidbody2D rb;
    private float lastTimePlayerInVicinity;
    private bool isFollowingPlayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastTimePlayerInVicinity = Time.time;
        isFollowingPlayer = false;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(anchor.position, player.position);

        if (distanceToPlayer <= vicinityRadius)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;

            // Move the head toward the player.
            rb.velocity = directionToPlayer * moveSpeed;

            // Smoothly rotate the head to face player
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            lastTimePlayerInVicinity = Time.time;
            isFollowingPlayer = true;

            //Debug.Log("Player is in vicinity. Following player.");
        }
        else
        {
            // Player is outside the vicinity.
            if (isFollowingPlayer && Time.time - lastTimePlayerInVicinity <= inertiaDuration)
            {
                // Apply inertia to keep moving for a short duration
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
            else
            {
                // Stop moving.
                rb.velocity = Vector2.zero;
                isFollowingPlayer = false;
                //Debug.Log("Player is outside radius");
            }
        }
    }
}