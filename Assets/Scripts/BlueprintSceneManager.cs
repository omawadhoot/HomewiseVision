using System;
using System.Collections.Generic;
using UnityEngine;
using Vuforia;
using Firebase.Firestore;
using Firebase.Auth;

public class BlueprintSceneManager : MonoBehaviour
{
    public ModelData modelData; // Reference to Scriptable Object

    [System.Serializable]
    public class ModelEntry
    {
        public string tag;
        public GameObject model;
    }

    public List<ModelEntry> modelEntries = new List<ModelEntry>();

    private ObserverBehaviour observerBehaviour;
    private Dictionary<string, GameObject> houseModels = new Dictionary<string, GameObject>();
    private FirebaseFirestore db;
    private FirebaseAuth auth;

    void Start()
    {
        observerBehaviour = GetComponent<ObserverBehaviour>();
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;

        if (observerBehaviour)
        {
            observerBehaviour.OnTargetStatusChanged += OnObserverStatusChanged;
        }
        else
        {
            Debug.LogError("ObserverBehaviour component is missing from this GameObject!");
        }

        foreach (var entry in modelEntries)
        {
            if (!houseModels.ContainsKey(entry.tag))
            {
                houseModels.Add(entry.tag, entry.model);
            }
        }
    }

    private void OnObserverStatusChanged(ObserverBehaviour observer, TargetStatus targetStatus)
    {
        Debug.Log("Observer Status Changed: " + targetStatus.Status); // Debug Log ✅

        if (targetStatus.Status == Status.TRACKED)
        {
            Debug.Log("Target Found! Calling OnTargetFound()..."); // Debug Log ✅
            OnTargetFound();
        }
    }

    private void OnTargetFound()
    {
        string detectedTag = observerBehaviour.TargetName;

        if (houseModels.ContainsKey(detectedTag))
        {
            Debug.Log("Blueprint scanned: " + detectedTag);
            modelData.SetModelTag(detectedTag); // ✅ Save the tag to ScriptableObject

            // ✅ Save to Firestore under the logged-in user's "history" collection
            SaveTagToFirestore(detectedTag);
        }
        else
        {
            Debug.LogWarning("No matching model found for tag: " + detectedTag);
        }
    }

    private void OnTargetLost()
    {
        modelData.modelTag = null;
    }

    // ✅ Function to Save the Model Tag to Firestore
    private void SaveTagToFirestore(string modelTag)
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("❌ No user logged in!");
            return;
        }

        string userId = auth.CurrentUser.UserId;
        DocumentReference historyRef = db.Collection("users").Document(userId).Collection("history").Document(modelTag);

        // ✅ Check if the document already exists to prevent duplicates
        historyRef.GetSnapshotAsync().ContinueWith(task =>
        {
            if (task.IsCompleted && !task.Result.Exists)
            {
                Dictionary<string, object> historyData = new Dictionary<string, object>
                {
                    { "tag", modelTag },
                    { "timestamp", FieldValue.ServerTimestamp } // ✅ Store the scan time
                };

                // ✅ Add the new scanned model tag to Firestore
                historyRef.SetAsync(historyData).ContinueWith(setTask =>
                {
                    if (setTask.IsCompleted)
                    {
                        Debug.Log("✅ Model Tag Saved to Firestore: " + modelTag);
                    }
                    else
                    {
                        Debug.LogError("❌ Failed to save model tag to Firestore: " + setTask.Exception);
                    }
                });
            }
            else
            {
                Debug.Log("ℹ️ Model Tag already exists in Firestore: " + modelTag);
            }
        });
    }
}
