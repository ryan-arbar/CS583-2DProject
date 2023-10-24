using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knockback : MonoBehaviour
{
    private Rigidbody2D rb;
    public int health = 3000;
    public int healthIntensity;
    public float knockbackPower = 5.0f;
    private Vector2 knockbackVelocity;
    public float knockbackDecayRate = 0.9f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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
            Debug.Log("Player is dead!");
            gameObject.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "borb")
        {
            Vector2 knockbackDirection = (transform.position - collision.transform.position).normalized;
            knockbackVelocity = knockbackDirection * knockbackPower;
            TakeDamage(1);
        }
    }

    private void Update()
    {
        // Decay the knockback velocity
        knockbackVelocity *= knockbackDecayRate;

        // Apply the knockback velocity
        rb.velocity += knockbackVelocity;
    }
}