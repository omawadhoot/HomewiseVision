using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class LogOutUser : MonoBehaviour
{
    private FirebaseAuth auth;

    void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
    }

    public void LogoutUser()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            SceneManager.LoadScene("Authentication");
            Debug.Log("✅ User logged out successfully.");
        }
        else
        {
            Debug.LogWarning("⚠️ No user is currently logged in.");
        }
    }
}
