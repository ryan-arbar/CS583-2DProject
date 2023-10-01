using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food_script : MonoBehaviour
{
    // Floating Parameters
    public float floatAmplitudeMin = 0.1f;
    public float floatAmplitudeMax = 0.3f;
    public float floatSpeedMin = 0.5f;
    public float floatSpeedMax = 2.0f;

    // Rotation Parameters
    public float rotationSpeedMin = 10.0f;
    public float rotationSpeedMax = 30.0f;

    // Wandering Parameters
    public float wanderDistance = 0.2f;
    private Vector3 startPosition;
    private float floatAmplitude;
    private float floatSpeed;
    private float rotationSpeed;
    private Vector3 wanderDirection;
    private Vector3 targetWanderDirection;
    private float wanderTimer;
    private float wanderChangeDuration = 2.0f;
    private float wanderChangeTimer;

    // Attraction Parameters
    public float attractionDistance = 1.0f;
    public float attractionSpeed = .5f;
    public float proximityToPlayerToDisappear = 0.5f;
    private Transform player;
    private bool isDisappearing = false;

    private Rigidbody2D rb;

    private void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Initialize floating and rotation parameters
        floatAmplitude = Random.Range(floatAmplitudeMin, floatAmplitudeMax);
        floatSpeed = Random.Range(floatSpeedMin, floatSpeedMax);
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);

        // Initialize wandering parameters
        wanderTimer = Random.Range(0f, Mathf.PI * 2f);
        wanderDirection = Random.insideUnitCircle.normalized;
        targetWanderDirection = wanderDirection;
        wanderChangeTimer = Random.Range(0f, wanderChangeDuration);

        // Exclude collisions between the food and player layers
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("FoodLayer"), LayerMask.NameToLayer("PlayerLayer"));
    }

    private void Update()
    {
        if (isDisappearing)
        {
            // Handle food disappearance
            HandleDisappearance();
        }
        else
        {
            // Calculate distance to the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer < attractionDistance)
            {
                // Handle attraction to the player
                HandleAttraction(distanceToPlayer);
            }
            else
            {
                // Handle wandering behavior
                HandleWandering();
            }
        }

        // Handle food rotation
        HandleRotation();
    }

    private void HandleDisappearance()
    {
        // The food is in the process of disappearing; you can add any desired disappearance animation here
        // For simplicity, we'll just disable the GameObject
        gameObject.SetActive(false);
    }

    private void HandleAttraction(float distanceToPlayer)
    {
        // Calculate the direction from the food to the player
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        // Move the food toward the player
        transform.position += directionToPlayer * attractionSpeed * Time.deltaTime;

        // Check if the food is close enough to disappear
        if (distanceToPlayer < proximityToPlayerToDisappear)
        {
            isDisappearing = true;
            // You can add any desired disappearance animation here
            // For simplicity, we'll just disable the GameObject
            gameObject.SetActive(false);
        }
    }

    private void HandleWandering()
    {
        // Update wandering behavior
        wanderTimer += Time.deltaTime * floatSpeed;

        // Gradually change the wander direction
        wanderChangeTimer += Time.deltaTime;
        if (wanderChangeTimer >= wanderChangeDuration)
        {
            targetWanderDirection = Random.insideUnitCircle.normalized;
            wanderChangeTimer = 0f;
        }
        wanderDirection = Vector3.Slerp(wanderDirection, targetWanderDirection, wanderChangeTimer / wanderChangeDuration);

        Vector3 wanderPosition = startPosition + wanderDirection * Mathf.Sin(wanderTimer) * wanderDistance;

        // Floating motion using a sine wave
        float newY = wanderPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        Vector3 newPosition = new Vector3(wanderPosition.x, newY, wanderPosition.z);

        // Set the velocity to move towards the new position
        rb.velocity = (newPosition - transform.position) * floatSpeed;
    }

    private void HandleRotation()
    {
        // Rotation
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }
}
