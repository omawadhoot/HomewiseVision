using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ModelEntry
{
    public string tag; // The tag associated with the model
    public GameObject model; // The 3D model GameObject
}

public class CostSceneManager : MonoBehaviour
{
    public ModelData modelData; // Reference to Scriptable Object
    [SerializeField] private List<ModelEntry> houseModelsList = new List<ModelEntry>(); // Use List for Inspector visibility
    private Dictionary<string, GameObject> houseModels = new Dictionary<string, GameObject>(); // This will hold the models by tag

    void Start()
    {
        // Convert List to Dictionary
        houseModels.Clear(); // Clear any existing data

        foreach (var entry in houseModelsList)
        {
            if (!houseModels.ContainsKey(entry.tag))
            {
                houseModels.Add(entry.tag, entry.model);
            }
        }

        string selectedTag = modelData.GetModelTag();

        Debug.Log("🔥 Retrieved Model Tag from ScriptableObject: " + selectedTag);

        if (!string.IsNullOrEmpty(selectedTag) && houseModels.ContainsKey(selectedTag))
        {
            houseModels[selectedTag].SetActive(true);
            Debug.Log("✅ Model activated in Cost Scene: " + selectedTag);
        }
        else
        {
            Debug.LogWarning("❌ No model found for tag: " + selectedTag);
        }
    }
}