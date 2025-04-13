using UnityEngine;

public class HouseManager : MonoBehaviour
{
    public Camera[] houseCameras; // Array to hold all 5 cameras
    public ModelData modelData;   // Reference to the ScriptableObject holding the modelTag

    private Camera activeCamera;

    // Method to set the active camera based on the modelTag
    public void SetActiveCameraBasedOnModelTag()
    {
        // Compare the modelTag with the camera tags
        foreach (Camera houseCamera in houseCameras)
        {
            if (houseCamera.CompareTag(modelData.GetModelTag()))
            {
                if (activeCamera != null)
                {
                    activeCamera.gameObject.SetActive(false); // Disable the previous active camera
                }

                activeCamera = houseCamera; // Set new camera as active
                activeCamera.gameObject.SetActive(true); // Enable the new active camera
                Debug.Log("✅ Active camera set to: " + houseCamera.name);
                return; // Exit once a match is found
            }
        }

        Debug.LogError("❌ No camera with matching tag found!");
    }

    //Optionally, a method to get the active camera
    public Camera GetActiveCamera()
    {
        return activeCamera;
    }
}