using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodCounter : MonoBehaviour
{
    public int foodCount = 0;
    private TextMeshProUGUI foodCountText;

    public void Start()
    {
        foodCountText = GetComponent<TextMeshProUGUI>();
        UpdateFoodCountText();
    }

    public void IncreaseFoodCount()
    {
        foodCount++;
        UpdateFoodCountText();
    }

    public void UpdateFoodCountText()
    {
        if (foodCountText != null)
        {
            foodCountText.text = "Food Eaten: " + foodCount;
        }
    }

    public void DecreaseFoodCount(int amount)
    {
        foodCount -= amount;
        if (foodCount < 0) foodCount = 0; // Prevent food count from going negative
        UpdateFoodCountText();
    }
}