using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public HistoryData historyData; // Reference to HistoryData ScriptableObject
    public List<GameObject> models; // List of all possible models in the scene

    void Start()
    {
        Debug.Log("🟢 Checking stored scanned models in HistoryData...");
    
        foreach (string modelTag in historyData.scannedModels)
        {
            Debug.Log($"🔹 Stored scanned model: {modelTag}");
        }

        ActivateScannedModels();
    }

    private void ActivateScannedModels()
    {
        foreach (GameObject model in models)
        {
            string tag = model.name; // Assuming model name matches tag in HistoryData
            
            if (historyData.IsModelScanned(tag))
            {
                model.SetActive(true);
                Debug.Log($"✅ Activated model: {tag}");
            }
            else
            {
                model.SetActive(false);
                Debug.Log($"❌ Model not scanned: {tag}");
            }
        }
    }
}