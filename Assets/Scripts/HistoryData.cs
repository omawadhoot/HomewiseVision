using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HistoryData", menuName = "ScriptableObjects/HistoryData", order = 1)]
public class HistoryData : ScriptableObject
{
    public List<string> scannedModels = new List<string>(); // Stores scanned model tags

    // ✅ Mark a model as scanned
    public void MarkModelAsScanned(string modelTag)
    {
        if (!scannedModels.Contains(modelTag))
        {
            scannedModels.Add(modelTag);
            Debug.Log($"📌 Model marked as scanned: {modelTag}");
            SaveChanges();
        }
    }

    // ❌ Remove a model from scanned history
    public void RemoveScannedModel(string modelTag)
    {
        if (scannedModels.Contains(modelTag))
        {
            scannedModels.Remove(modelTag);
            Debug.Log($"🗑️ Removed scanned model: {modelTag}");
            SaveChanges();
        }
    }

    // 🔍 Check if a model was scanned
    public bool IsModelScanned(string modelTag)
    {
        return scannedModels.Contains(modelTag);
    }

    // 🔥 Save changes in Unity Editor mode
    private void SaveChanges()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
}