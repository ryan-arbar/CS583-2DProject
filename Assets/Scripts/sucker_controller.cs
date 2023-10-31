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

    private Rigidbody2D rb;

    public FoodCounter foodCounter;
    public int scoreDecreaseAmount = 1;
    public float scoreDecreaseInterval = 1f;
    private float lastTimeScoreDecreased;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        lastTimeScoreDecreased = Time.time;
    }

    private void Update()
    {
        float distanceToPlayer = Vector2.Distance(anchor.position, player.position);

        if (distanceToPlayer <= vicinityRadius)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;

            // Move head toward player
            rb.velocity = directionToPlayer * moveSpeed;

            // Smoothly rotate the head to face player
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            //Debug.Log("Player is in vicinity. Following player.");
        }
        else
        {
            // Stop moving
            rb.velocity = Vector2.zero;
            //Debug.Log("Player is outside radius");
            
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure the player's tag is set to "Player"
        {
            if (Time.time - lastTimeScoreDecreased > scoreDecreaseInterval)
            {
                if (foodCounter != null)
                {
                    foodCounter.DecreaseFoodCount(scoreDecreaseAmount);
                    lastTimeScoreDecreased = Time.time;
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (anchor != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(anchor.position, vicinityRadius);
        }
    }
}