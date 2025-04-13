using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomiseSceneManager : MonoBehaviour
{
    public ModelData modelData;

    public List<ModelEntry> houseModelsList = new List<ModelEntry>(); 
    private Dictionary<string, GameObject> houseModels = new Dictionary<string, GameObject>();

    void Start()
    {
        // Convert List to Dictionary
        houseModels.Clear(); 

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
            Debug.Log("✅ Model activated in Customisation Scene: " + selectedTag);
        }
        else
        {
            Debug.LogWarning("❌ No model found for tag: " + selectedTag);
        }
    }
}