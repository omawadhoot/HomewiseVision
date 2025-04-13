using UnityEngine;

public class FurniturePlacer : MonoBehaviour
{
    public Camera[] cameras;  // Assign all 5 cameras in the Inspector
    public GameObject[] furniturePrefabs;  // Assign different furniture prefabs
    public Transform furnitureParent;  // Parent object to keep hierarchy clean
    public float spawnDistance = 2f;  // Distance from the active camera

    public void SpawnFurniture(int furnitureIndex)
    {
        if (furnitureIndex < 0 || furnitureIndex >= furniturePrefabs.Length)
        {
            Debug.LogWarning("❌ Invalid furniture index!");
            return;
        }

        Camera activeCamera = GetActiveCamera();
        if (activeCamera != null)
        {
            SpawnObject(activeCamera, furnitureIndex);
        }
        else
        {
            Debug.LogWarning("❌ No active camera found!");
        }
    }

    Camera GetActiveCamera()
    {
        foreach (Camera cam in cameras)
        {
            if (cam.gameObject.activeInHierarchy)
            {
                return cam;
            }
        }
        return null;
    }

    void SpawnObject(Camera activeCamera, int index)
    {
        // Calculate the spawn position just in front of the active camera
        Vector3 spawnPosition = activeCamera.transform.position + activeCamera.transform.forward * spawnDistance;

        // Ensure the object faces the correct direction
        Quaternion spawnRotation = Quaternion.LookRotation(activeCamera.transform.forward);

        // Instantiate the object at the calculated position with rotation
        GameObject newFurniture = Instantiate(furniturePrefabs[index], spawnPosition, spawnRotation, furnitureParent);

        // Add a draggable script if required
        newFurniture.AddComponent<FurnitureDraggable>();

        Debug.Log("✅ Furniture Spawned: " + furniturePrefabs[index].name + " at " + spawnPosition);
    }

}
