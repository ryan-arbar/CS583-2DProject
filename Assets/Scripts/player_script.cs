using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_script : MonoBehaviour
{
    public float swimSpeed = 3.0f;
    public float dashPower = 10.0f; // Force applied when pressing spacebar for a dash
    public float dashDuration = 0.2f; // Duration of the dash
    public float dashCooldown = 1.0f; // Cooldown period between dashes
    public float rotationDamping = 0.4f; // Controls how quickly the player stops spinning after a collision
    public float directionChangeSmoothness = 2.3f; // Controls the smoothness of direction changes when floating freely
    public float objectContactDirectionChangeSmoothness = 5.0f; // Controls the smoothness of direction changes when in contact with objects
    public float stoppingDeceleration = 1.0f; // Controls how quickly the player stops when no input is given
    public LayerMask wallLayer;

    private Rigidbody2D rb;
    private bool isTouchingObject = false;
    private bool canDash = true;
    private bool isDashing;
    private bool isBouncing = false;

    public int health = 3000;
    public int healthIntensity;
    public float knockbackPower = 5.0f;
    private Vector2 knockbackVelocity;
    public float knockbackDecayRate = 0.9f; // Adjust this for faster/slower decay. Closer to 1.0 means slower decay.



    [SerializeField] private TrailRenderer trail;

    private Vector2 targetVelocity;

    private void Start()
    {
        trail = GetComponent<TrailRenderer>();

        rb = GetComponent<Rigidbody2D>();
        targetVelocity = Vector2.zero;
    }

    private void Update()
    {
        // Handle player movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Gradually adjust the target velocity based on the current context
        float currentDirectionChangeSmoothness = isTouchingObject ? objectContactDirectionChangeSmoothness : directionChangeSmoothness;
        Vector2 inputDirection = new Vector2(horizontalInput, verticalInput).normalized;
        targetVelocity = Vector2.Lerp(targetVelocity, inputDirection * swimSpeed, Time.deltaTime * currentDirectionChangeSmoothness);

        // Apply deceleration when no input is given
        if (inputDirection.magnitude < 0.1f)
        {
            targetVelocity = Vector2.Lerp(targetVelocity, Vector2.zero, Time.deltaTime * stoppingDeceleration);
        }

        // Apply the target velocity when the player isn't dashing
        if (!isDashing)
        {
            rb.velocity = targetVelocity + knockbackVelocity;
        }

        // Decay the knockback velocity
        knockbackVelocity *= knockbackDecayRate;

        // If not touching an object, apply rotational damping
        if (!isTouchingObject)
        {
            rb.angularVelocity *= (1.0f - Time.deltaTime * rotationDamping);
        }

        // Handle propulsion when pressing the spacebar
        if (Input.GetKeyDown(KeyCode.Space) && canDash && !isBouncing)
        {
            // Start a dash
            StartCoroutine(Dash());
        }
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;
        CheckDeath();
    }

    private void CheckDeath()
    {
        if (health <= 0)
        {
            // Here, you can trigger any event you want when the player "dies"
            Debug.Log("Player is dead!");

            // For now, I'll just deactivate the player
            this.gameObject.SetActive(false);
        }
    }


    

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Reset the flag when no longer colliding with an object
        isTouchingObject = false;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 dashDirection = new Vector2(horizontalInput, verticalInput).normalized;


        // Define rayStart here
        Vector2 rayStart = (Vector2)transform.position + dashDirection * 0.1f;  // Adjust the 0.1f offset as needed
        // Raycast in the dash direction to check for obstacles
        Debug.DrawRay(rayStart, dashDirection * dashPower, Color.red, 5f);
        RaycastHit2D hit = Physics2D.Raycast(rayStart, dashDirection, dashPower - 0.1f, wallLayer);


        if (hit.collider != null)
        {
            Debug.Log("Raycast hit: " + hit.collider.gameObject.name);
        }
        else
        {
            rb.velocity = dashDirection * dashPower;
        }

        rb.velocity = dashDirection * dashPower;


        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
        StartCoroutine(DashCooldown());
    }

    private IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        float originalGravity = rb.gravityScale;

        if (collision.gameObject.tag == "borb")
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            knockbackVelocity = knockbackDirection * knockbackPower; // Use the knockbackPower variable you have

            TakeDamage(1);
        }

        // Set the flag when colliding with an object
        isTouchingObject = true;



    }
}