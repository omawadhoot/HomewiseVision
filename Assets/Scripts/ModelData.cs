using UnityEngine;

[CreateAssetMenu(fileName = "ModelData", menuName = "ScriptableObjects/ModelData", order = 1)]
public class ModelData : ScriptableObject
{
    public string modelTag; // Make it private to enforce encapsulation

    public void SetModelTag(string tag)
    {
        if (!string.IsNullOrEmpty(tag)) // Prevent empty or null tags
        {
            modelTag = tag;
            Debug.Log("✅ Model tag saved in ScriptableObject: " + modelTag);

            // 🔥 Force Unity to Save the ScriptableObject at Runtime (Editor Only)
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
        else
        {
            Debug.LogError("❌ Attempted to set an empty model tag!");
        }
    }

    public string GetModelTag()
    {
        if (string.IsNullOrEmpty(modelTag))
        {
            Debug.LogWarning("⚠️ Model tag is empty!");
        }
        return modelTag;
    }

    public void ClearModelTag()
    {
        modelTag = "";
        Debug.Log("🗑️ Cleared model tag in ScriptableObject.");

        // Save changes
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }
}