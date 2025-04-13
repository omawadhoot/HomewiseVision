using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelRotationManager : MonoBehaviour
{
    [SerializeField] private GameObject[] houseModels;
    public float rotationSpeed = 50f; // Rotation speed

    void Update()
    {
        foreach (var model in houseModels)
        {
            if (model.activeInHierarchy) // Check if the model is active
            {
                RotateModel(model);
            }
        }
    }

    private void RotateModel(GameObject model)
    {
        model.transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // Rotate around its own Y-axis
    }
}