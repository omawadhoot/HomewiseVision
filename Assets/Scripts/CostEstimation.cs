using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

public class CostEstimation : MonoBehaviour
{
    // Parameters
    [SerializeField] private TextMeshProUGUI totalCost;
    [SerializeField] private TMP_InputField slabInput, areaInput, costInput;
    private int totalCarpetArea;
    private double _totalCost;

    // Start is called before the first frame update
    void Start()
    {
        // Initially all the fields must be empty
        slabInput.text = string.Empty;
        areaInput.text = string.Empty;
        costInput.text = string.Empty;
        totalCost.text = string.Empty;
    }

    // Update is called once per frame
    void Update()
    {
        // Validate inputs before parsing
        if (!string.IsNullOrWhiteSpace(slabInput.text) &&
            !string.IsNullOrWhiteSpace(areaInput.text) &&
            !string.IsNullOrWhiteSpace(costInput.text))
        {
            CalculateTotalCost();
        }
    }

    private void CalculateTotalCost()
    {
        // Use TryParse to prevent exceptions
        if (int.TryParse(slabInput.text, out int slab) &&
            int.TryParse(areaInput.text, out int area) &&
            double.TryParse(costInput.text, NumberStyles.Float, CultureInfo.InvariantCulture, out double cost))
        {
            totalCarpetArea = slab * area;
            _totalCost = totalCarpetArea * cost;
            totalCost.text = $"The total cost of your dream home will be INR {_totalCost:N2}!";
        }
        else
        {
            totalCost.text = "Invalid input! Please enter valid numbers.";
        }
    }
}
