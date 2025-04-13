using UnityEngine;
using Firebase;
using Firebase.Firestore;
using Firebase.Auth;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class HistorySceneManager : MonoBehaviour
{
    public GameObject historyItemPrefab;
    public Transform contentPanel;
    public HistoryData historyData; // Reference to HistoryData

    private FirebaseFirestore db;
    private FirebaseAuth auth;

    void Start()
    {
        db = FirebaseFirestore.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
        LoadHistory();
    }

    private void LoadHistory()
    {
        if (auth.CurrentUser == null)
        {
            Debug.LogError("❌ No user logged in!");
            return;
        }

        string userId = auth.CurrentUser.UserId;
        StartCoroutine(LoadHistoryCoroutine(userId));
    }

    private IEnumerator LoadHistoryCoroutine(string userId)
    {
        var task = db.Collection("users").Document(userId).Collection("history").GetSnapshotAsync();
        yield return new WaitUntil(() => task.IsCompleted);

        if (task.IsCompleted)
        {
            QuerySnapshot snapshot = task.Result;
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                string modelTag = document.GetValue<string>("tag");
                Debug.Log("📌 Found model tag: " + modelTag);

                // ✅ Store scanned model in HistoryData
                historyData.MarkModelAsScanned(modelTag);

                CreateHistoryItem(modelTag);
            }
        }
        else
        {
            Debug.LogError("❌ Failed to load history: " + task.Exception);
        }
    }

    private void CreateHistoryItem(string modelTag)
    {
        GameObject historyItem = Instantiate(historyItemPrefab, contentPanel);
        TextMeshProUGUI tagText = historyItem.GetComponentInChildren<TextMeshProUGUI>();
        tagText.text = modelTag;

        Button switchSceneButton = historyItem.transform.Find("SwitchSceneButton").GetComponent<Button>();
        Button deleteButton = historyItem.transform.Find("DeleteButton").GetComponent<Button>();

        switchSceneButton.onClick.AddListener(() => SwitchScene(modelTag));
        deleteButton.onClick.AddListener(() => DeleteHistoryItem(modelTag, historyItem));
    }

    private void SwitchScene(string modelTag)
    {
        historyData.MarkModelAsScanned(modelTag); // Ensure it's marked
        SceneManager.LoadScene("ArtificialEnvironment");
    }

    private void DeleteHistoryItem(string modelTag, GameObject historyItem)
    {
        DeleteTagFromFirestore(modelTag);
        Destroy(historyItem);
        historyData.RemoveScannedModel(modelTag); // Remove from scanned list
    }

    private void DeleteTagFromFirestore(string modelTag)
    {
        if (auth.CurrentUser == null) return;

        string userId = auth.CurrentUser.UserId;
        db.Collection("users").Document(userId).Collection("history").Document(modelTag).DeleteAsync()
            .ContinueWith(task =>
            {
                if (task.IsCompleted)
                    Debug.Log("✅ Deleted tag: " + modelTag);
                else
                    Debug.LogError("❌ Deletion failed: " + task.Exception);
            });
    }
}