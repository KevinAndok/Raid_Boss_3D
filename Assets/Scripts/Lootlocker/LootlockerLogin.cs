using System.Collections.Generic;
using UnityEngine.EventSystems;
using LootLocker.Requests;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class LootlockerLogin : MonoBehaviour
{
    public static int? playerID;
    public static string playerName;

    [SerializeField] private TMP_Text HeaderText;

    [SerializeField] private TMP_InputField Name;
    [SerializeField] private TMP_InputField Email;
    [SerializeField] private TMP_InputField Password;
    [SerializeField] private Button ConfirmButton;
    [SerializeField] private TMP_Text ConfirmButtonText;
    
    [SerializeField] private GameObject LoginCanvas;
    [SerializeField] private GameObject LoginScreenButton;

    [SerializeField] private Image[] ScreenButtons;

    [SerializeField] private List<GameObject> _tabbable = new List<GameObject>(); //lol

    private void Awake()
    {
        playerID = null;
        playerName = null;
    }
    private void Start()
    {
        CheckPreviousSession();
    }

    #region Sessions
    public void StartSession()
    {
        LootLockerSDKManager.StartWhiteLabelSession((response) =>
        {
            if (!response.success)
            {
                Debug.Log("error starting LootLocker session");
                return;
            }
            Debug.Log("session started successfully");

            playerID = response.player_id;
            if (playerName != null) SetPlayerName(playerName);
            else playerName = GetPlayerName();
            //change scene
            LoginCanvas.SetActive(false);
        });
    }
    public void CheckPreviousSession()
    {
        LootLockerSDKManager.CheckWhiteLabelSession(response =>
        {
            if (response)
            {
                // Start a new session
                Debug.Log("session is valid, you can start a game session");
                StartSession();
            }
            else
            {
                // Show login form here
                Debug.Log("session is NOT valid, we should show the login form");
                LoginCanvas.SetActive(true);
                LoginScreen(LoginScreenButton);
            }
        });
    }
    #endregion

    #region Login Functions
    public void SignUp()
    {
        if (Name.text == "") return;
        if (Email.text == "") return;
        if (Password.text == "") return;
        if (Password.text.Length < 8) return;

        playerName = Name.text;

        LootLockerSDKManager.WhiteLabelSignUp(Email.text, Password.text, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error while creating user");
                return;
            }

            Debug.Log("user created successfully");

            Login();

        });
    }
    public void Login()
    {
        if (Email.text == "") return;
        if (Password.text == "") return;

        LootLockerSDKManager.WhiteLabelLogin(Email.text, Password.text, false, response =>
        {
            if (!response.success)
            {
                Debug.Log("error while logging in");
                return;
            }

            string token = response.SessionToken;
            // Start game session here
            StartSession();
        });
    }
    public void ResetPassword()
    {
        if (Email.text == "") return;

        LootLockerSDKManager.WhiteLabelRequestPassword(Email.text, (response) =>
        {
            if (!response.success)
            {
                Debug.Log("error requesting password reset");
                return;
            }

            Debug.Log("requested password reset successfully");
        });
    }
    #endregion

    #region Screens
    public void SignUpScreen(GameObject self)
    {
        SetTargetScreenButton(self);

        Name.gameObject.SetActive(true);
        Email.gameObject.SetActive(true);
        Password.gameObject.SetActive(true);
        ConfirmButton.gameObject.SetActive(true);

        HeaderText.text = "Create an Account";
        ConfirmButtonText.text = "Sign Up";

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(SignUp);

        EventSystem.current.SetSelectedGameObject(Name.gameObject);
    }
    public void LoginScreen(GameObject self)
    {
        SetTargetScreenButton(self);

        Name.gameObject.SetActive(false);
        Email.gameObject.SetActive(true);
        Password.gameObject.SetActive(true);
        ConfirmButton.gameObject.SetActive(true);

        HeaderText.text = "Login";
        ConfirmButtonText.text = "Login";

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(Login);

        EventSystem.current.SetSelectedGameObject(Email.gameObject);
    }
    public void ResetPasswordScreen(GameObject self)
    {
        SetTargetScreenButton(self);

        Name.gameObject.SetActive(false);
        Email.gameObject.SetActive(true);
        Password.gameObject.SetActive(false);
        ConfirmButton.gameObject.SetActive(true);

        HeaderText.text = "Reset Password";
        ConfirmButtonText.text = "Reset";

        ConfirmButton.onClick.RemoveAllListeners();
        ConfirmButton.onClick.AddListener(ResetPassword);

        EventSystem.current.SetSelectedGameObject(Email.gameObject);
    }
    void SetTargetScreenButton(GameObject obj)
    {
        foreach (Image img in ScreenButtons)
        {
            if (img.gameObject == obj) img.color = new Color(1, 1, 1, 1);
            else img.color = new Color(1, 1, 1, .2f);
        }
    }
    #endregion

    #region Player Name
    public string GetPlayerName()
    {
        string name = string.Empty;

        LootLockerSDKManager.GetPlayerName((response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully retrieved player name: " + response.name);
                name = response.name;
            }
            else
            {
                Debug.Log("Error getting player name");
            }
        });

        return name;
    }
    public void SetPlayerName(string name)
    {
        LootLockerSDKManager.SetPlayerName(name, (response) =>
        {
            if (response.success)
            {
                Debug.Log("Successfully set player name");
            }
            else
            {
                Debug.Log("Error setting player name");
            }
        });
    }
    #endregion

    #region Input
    public void OnTab()
    {
        for (int i = 0; i < _tabbable.Count; i++)
        {
            if (EventSystem.current.currentSelectedGameObject == _tabbable[i])
            {
                GameObject next;
                do
                {
                    next = _tabbable[(++i) % _tabbable.Count];
                } while (!next.activeInHierarchy);

                EventSystem.current.SetSelectedGameObject(next);
                break;
            }
        }
    }
    #endregion
}
