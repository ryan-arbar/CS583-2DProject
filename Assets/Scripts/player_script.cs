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

    private Rigidbody2D rb;
    private bool isTouchingObject = false;
    private bool canDash = true;
    private bool isDashing;

    [SerializeField] private TrailRenderer tr;

    private Vector2 targetVelocity;

    private void Start()
    {
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
            rb.velocity = targetVelocity;
        }

        // If not touching an object, apply rotational damping
        if (!isTouchingObject)
        {
            rb.angularVelocity *= (1.0f - Time.deltaTime * rotationDamping);
        }

        // Handle propulsion when pressing the spacebar
        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            // Start a dash
            StartCoroutine(Dash());
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Set the flag when colliding with an object
        isTouchingObject = true;
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

        // Dash in the current movement direction
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector2 dashDirection = new Vector2(horizontalInput, verticalInput).normalized;
        rb.velocity = dashDirection * dashPower;

        tr.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}