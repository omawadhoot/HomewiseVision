using UnityEngine;
using UnityEngine.UI;

public class AuthSectionsManager : MonoBehaviour
{
    [SerializeField] private FirebaseAuthManager authManager;
    [SerializeField] private GameObject back;
    [SerializeField] private GameObject[] authSections;
    [SerializeField] private Button forgotPassword, signUp, login, register, sendLink;

    private void Start()
    {
        if (authManager == null) Debug.LogError("FirebaseAuthManager is not assigned!");
        if (forgotPassword == null || signUp == null || login == null || register == null || sendLink == null)
            Debug.LogError("One or more buttons are not assigned in the Inspector!");
        if (authSections == null || authSections.Length == 0)
            Debug.LogError("authSections array is empty or not assigned!");

        AuthInitializer();
    }

    private void AuthInitializer()
    {
        forgotPassword.onClick.RemoveAllListeners();
        signUp.onClick.RemoveAllListeners();
        login.onClick.RemoveAllListeners();
        register.onClick.RemoveAllListeners();
        sendLink.onClick.RemoveAllListeners();

        back.SetActive(false);
        SetActiveSection(0);

        login.onClick.AddListener(authManager.Login);

        signUp.onClick.AddListener(() =>
        {
            SetActiveSection(1);
            back.SetActive(true);
        });

        register.onClick.AddListener(authManager.SignUp);

        forgotPassword.onClick.AddListener(() =>
        {
            SetActiveSection(2);
            back.SetActive(true);
            sendLink.onClick.AddListener(authManager.ForgotPassword);
        });
    }

    private void SetActiveSection(int index)
    {
        if (index < 0 || index >= authSections.Length)
        {
            Debug.LogError("SetActiveSection index is out of bounds!");
            return;
        }

        for (int i = 0; i < authSections.Length; i++)
        {
            authSections[i].SetActive(i == index);
        }
    }
}