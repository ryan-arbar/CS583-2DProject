using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class player_script : MonoBehaviour
{
    public TextMeshProUGUI congratulationsText;

    public float swimSpeed = 3.0f;
    public float dashPower = 10.0f;
    private float dashDuration = 0.2f;
    public float dashCooldown = 1.0f;
    private float rotationDamping = 0.4f;
    private float directionChangeSmoothness = 2.3f;
    private float objectContactDirectionChangeSmoothness = 5.0f;
    private float stoppingDeceleration = 1.0f;
    public LayerMask wallLayer;

    private Rigidbody2D rb;
    private bool isTouchingObject = false;
    private bool canDash = true;
    private bool isDashing;
    private bool isBouncing = false;

    public int health = 3000;
    private float borbKnockbackPower = 5.0f;
    private Vector2 knockbackVelocity;
    private float knockbackDecayRate = 0.9f;

    public int borbDamage = 1;
    public int sporbDamage = 1;
    private float sporbKnockbackPower = 6.0f;

    public FoodCounter foodCounter;


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
        // Movement
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        float currentDirectionChangeSmoothness = isTouchingObject ? objectContactDirectionChangeSmoothness : directionChangeSmoothness;
        Vector2 inputDirection = new Vector2(horizontalInput, verticalInput).normalized;
        targetVelocity = Vector2.Lerp(targetVelocity, inputDirection * swimSpeed, Time.deltaTime * currentDirectionChangeSmoothness);

        if (inputDirection.magnitude < 0.1f)
        {
            targetVelocity = Vector2.Lerp(targetVelocity, Vector2.zero, Time.deltaTime * stoppingDeceleration);
        }

        if (!isDashing)
        {
            rb.velocity = targetVelocity + knockbackVelocity;
        }

        knockbackVelocity *= knockbackDecayRate;

        if (!isTouchingObject)
        {
            rb.angularVelocity *= (1.0f - Time.deltaTime * rotationDamping);
        }

        // Dash
        if (Input.GetKeyDown(KeyCode.Space) && canDash && !isBouncing)
        {
            // Start a dash
            StartCoroutine(Dash());
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            health = 30000;
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
            this.gameObject.SetActive(false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
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


        // Define rayStart
        Vector2 rayStart = (Vector2)transform.position + dashDirection * 0.1f; 
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

        // Borb handling
        if (collision.gameObject.tag == "borb")
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            knockbackVelocity = knockbackDirection * borbKnockbackPower;
            TakeDamage(borbDamage);
        }

        // Sporb handling
        if (collision.gameObject.tag == "sporb")
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            knockbackVelocity = knockbackDirection * sporbKnockbackPower; 
            TakeDamage(sporbDamage);
        }

        if (collision.gameObject.tag == "gorb")
        {
            Debug.Log("Gorb hit! Food count: " + foodCounter.foodCount);
            if (foodCounter.foodCount >= 100)
            {
                Destroy(collision.gameObject);
            }
        }

        // Gorb handling (kill with 100 or more food)
        if (collision.gameObject.tag == "gorb" && foodCounter.foodCount >= 100)
        {
            Destroy(collision.gameObject);

            if (congratulationsText != null)
            {
                congratulationsText.gameObject.SetActive(true);
            }
        }

        isTouchingObject = true;
    }
}