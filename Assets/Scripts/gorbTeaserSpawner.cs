using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gorbTeaserSpawner : MonoBehaviour
{
    public GameObject enemy;
    private bool hasSpawned = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            ToggleEnemyActivation();
            hasSpawned = true; // Set to true so it won't activate again
        }
    }

    private void ToggleEnemyActivation()
    {
        if (enemy != null)
        {
            enemy.SetActive(!enemy.activeSelf);
        }
        else
        {
            Debug.LogError("Enemy not set!");
        }
    }
}
