using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class food_script : MonoBehaviour
{
    public AudioClip pop;

    // Floating
    public float floatAmplitudeMin = 0.1f;
    public float floatAmplitudeMax = 0.3f;
    public float floatSpeedMin = 0.5f;
    public float floatSpeedMax = 2.0f;

    // Rotation
    public float rotationSpeedMin = 10.0f;
    public float rotationSpeedMax = 30.0f;

    // Wandering
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

    public float attractionDistance = 1.0f;
    public float attractionSpeed = .5f;
    public float proximityToPlayerToDisappear = 0.5f;
    private Transform player;

    private Rigidbody2D rb;

    private void Start()
    {
        startPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player").transform;

        floatAmplitude = Random.Range(floatAmplitudeMin, floatAmplitudeMax);
        floatSpeed = Random.Range(floatSpeedMin, floatSpeedMax);
        rotationSpeed = Random.Range(rotationSpeedMin, rotationSpeedMax);

        wanderTimer = Random.Range(0f, Mathf.PI * 2f);
        wanderDirection = Random.insideUnitCircle.normalized;
        targetWanderDirection = wanderDirection;
        wanderChangeTimer = Random.Range(0f, wanderChangeDuration);

        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("FoodLayer"), LayerMask.NameToLayer("PlayerLayer"));
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer < attractionDistance)
        {
            HandleAttraction(distanceToPlayer);
        }
        else
        {
            HandleWandering();
            HandleWandering();
        }
        
        HandleRotation();
    }

    private void HandleAttraction(float distanceToPlayer)
    {
        Vector3 directionToPlayer = (player.position - transform.position).normalized;

        transform.position += directionToPlayer * attractionSpeed * Time.deltaTime;

        if (distanceToPlayer < proximityToPlayerToDisappear)
        {
            eat_sound.Instance.PlaySound(pop);
            gameObject.SetActive(false);
        }
    }

    private void HandleWandering()
    {
        wanderTimer += Time.deltaTime * floatSpeed;

        wanderChangeTimer += Time.deltaTime;
        if (wanderChangeTimer >= wanderChangeDuration)
        {
            targetWanderDirection = Random.insideUnitCircle.normalized;
            wanderChangeTimer = 0f;
        }
        wanderDirection = Vector3.Slerp(wanderDirection, targetWanderDirection, wanderChangeTimer / wanderChangeDuration);

        Vector3 wanderPosition = startPosition + wanderDirection * Mathf.Sin(wanderTimer) * wanderDistance;

        float newY = wanderPosition.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
        Vector3 newPosition = new Vector3(wanderPosition.x, newY, wanderPosition.z);

        rb.velocity = (newPosition - transform.position) * floatSpeed;
    }

    private void HandleRotation()
    {
        transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
    }

    private void OnDisable()
    {
        FoodCounter foodCounter = FindObjectOfType<FoodCounter>();
        if (foodCounter != null)
        {
            foodCounter.IncreaseFoodCount();
        }
    }

}
