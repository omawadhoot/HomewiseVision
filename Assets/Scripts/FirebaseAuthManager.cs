using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase.Extensions;

public class FirebaseAuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;
    private FirebaseFirestore db;

    [SerializeField] private TMP_InputField emailInputLogin, passwordInputLogin;
    [SerializeField] private TextMeshProUGUI statusTextLogin;

    [SerializeField] private TMP_InputField emailInputSignUp, passwordInputSignUp, usernameInputSignUp;
    [SerializeField] private TextMeshProUGUI statusTextSignUp;

    [SerializeField] private TMP_InputField emailInputResetPassword;
    [SerializeField] private TextMeshProUGUI statusTextResetPassword;

    void Awake()
    {
        Debug.Log("Initializing Firebase...");
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;

        if (auth != null && db != null)
        {
            Debug.Log("Firebase successfully initialized!");
        }
        else
        {
            Debug.LogError("Firebase initialization failed!");
        }
    }

    public void Login()
    {
        string email = emailInputLogin.text.Trim();
        string password = passwordInputLogin.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            statusTextLogin.text = "Please enter both email and password.";
            return;
        }

        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                statusTextLogin.text = "Login failed. Check credentials.";
                return;
            }

            user = task.Result.User;
            Debug.Log($"✅ Login successful! User ID: {user.UserId}");
            statusTextLogin.text = "Login successful!";

            SceneManager.LoadScene("LandingPage");
        });
    }

    public void SignUp()
    {
        string email = emailInputSignUp.text.Trim();
        string password = passwordInputSignUp.text.Trim();
        string username = usernameInputSignUp.text.Trim();

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(username))
        {
            statusTextSignUp.text = "All fields are required.";
            return;
        }

        if (password.Length < 6)
        {
            statusTextSignUp.text = "Password must be at least 6 characters.";
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                statusTextSignUp.text = "Signup failed. Try a different email.";
                return;
            }

            user = task.Result.User;
            SaveUserToFirestore(user.UserId, username, email);
        });
    }

    void SaveUserToFirestore(string userId, string username, string email)
    {
        DocumentReference userRef = db.Collection("users").Document(userId);
        Dictionary<string, object> userData = new Dictionary<string, object>
        {
            { "username", username },
            { "email", email },
            { "createdAt", FieldValue.ServerTimestamp }
        };

        userRef.SetAsync(userData).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Error saving user to Firestore.");
                return;
            }

            Debug.Log("✅ User successfully saved to Firestore.");
            SceneManager.LoadScene("LandingPage");
        });
    }

    public void ForgotPassword()
    {
        string email = emailInputResetPassword.text.Trim();

        if (string.IsNullOrEmpty(email))
        {
            statusTextResetPassword.text = "Enter your email to reset password.";
            return;
        }

        auth.SendPasswordResetEmailAsync(email).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                statusTextResetPassword.text = "Failed to send reset link.";
                return;
            }

            statusTextResetPassword.text = "Reset link sent. Check your email.";
        });
    }

    // 🔴 **Logout Method**
    public void LogoutUser()
    {
        if (auth.CurrentUser != null)
        {
            auth.SignOut();
            Debug.Log("✅ User logged out successfully.");
            SceneManager.LoadScene("LoginPage"); // Redirect to login screen
        }
        else
        {
            Debug.LogWarning("⚠️ No user is currently logged in.");
        }
    }
}