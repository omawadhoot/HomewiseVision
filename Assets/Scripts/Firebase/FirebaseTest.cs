using System;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;

public class FirebaseTest : MonoBehaviour
{
    void Start()
    {
        CheckFirebaseConnection();
    }

    async void CheckFirebaseConnection()
    {
        // Initialize Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
            FirebaseApp app = FirebaseApp.DefaultInstance;

            if (task.Result == DependencyStatus.Available)
            {
                Debug.Log("Firebase is initialized successfully!");
                TestAuthentication();
                TestFirestore();
            }
            else
            {
                Debug.LogError($"Firebase initialization failed: {task.Result}");
            }
        });
    }

    void TestAuthentication()
    {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        if (auth != null)
        {
            Debug.Log("Firebase Authentication is working!");
        }
        else
        {
            Debug.LogError("Firebase Authentication failed to initialize.");
        }
    }

    async void TestFirestore()
    {
        FirebaseFirestore db = FirebaseFirestore.DefaultInstance;
        if (db != null)
        {
            Debug.Log("Firebase Firestore is working!");

            // Test writing a document to Firestore
            //DocumentReference docRef = db.Collection("testCollection").Document("testDocument");
            //await docRef.SetAsync(new { message = "Hello from Unity!" })
            //    .ContinueWith(task =>
            //    {
            //        if (task.IsCompleted)
            //        {
            //            Debug.Log("Firestore test document added successfully!");
            //        }
            //        else
            //        {
            //            Debug.LogError("Firestore test failed: " + task.Exception);
            //        }
            //    });
        }
        else
        {
            Debug.LogError("Firebase Firestore failed to initialize.");
        }
    }
}
