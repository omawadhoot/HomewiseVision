using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HideUi : MonoBehaviour
{
    
    [SerializeField] private GameObject[] houses;
    [SerializeField] private GameObject viewCost, customise, scanningText;

    void Start()
    {

    }

    void Update()
    {
        bool isActive = false;

        foreach (GameObject house in houses)
        {
            if (house.activeInHierarchy)
            {
                isActive = true;
                break;
            }
        }

        if (isActive)
        {
            scanningText.SetActive(true);
            viewCost.SetActive(true);
            customise.SetActive(true);
        }
        else
        {
            viewCost.SetActive(false);
            customise.SetActive(false);
            scanningText.SetActive(true);
        }
    }
}