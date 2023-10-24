using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDisplay : MonoBehaviour
{
    public player_script player;
    private TextMeshProUGUI healthText;

    void Start()
    {
        healthText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (player != null)
        {
            healthText.text = "Health: " + player.health;
        }
    }
}
