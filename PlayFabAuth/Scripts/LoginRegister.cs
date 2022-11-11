using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LoginRegister : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TextMeshProUGUI displayText;
    public GameObject canvas;

    public UnityEvent onLoggedIn;

    public static LoginRegister instance;
    
    [HideInInspector]
    public string playFabId;
    
    void Awake()
    {
        canvas.SetActive(true);
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void OnRegister()
    {   
        RegisterPlayFabUserRequest registerRequest = new RegisterPlayFabUserRequest
        {
            Username = usernameInput.text,
            DisplayName = usernameInput.text,
            Password = passwordInput.text,
            RequireBothUsernameAndEmail = false
        };

        PlayFabClientAPI.RegisterPlayFabUser(registerRequest,
            result =>
            {
                SetDisplayText(result.PlayFabId, Color.green);
            },
            error =>
            {
                SetDisplayText(error.ErrorMessage, Color.red);
            }
        );
    }

    public void OnLogin()
    {
        LoginWithPlayFabRequest loginRequest = new LoginWithPlayFabRequest
        {
            Username = usernameInput.text,
            Password = passwordInput.text
        };

        PlayFabClientAPI.LoginWithPlayFab(loginRequest,
            result =>
            {
                SetDisplayText("Logged in as: " + result.PlayFabId, Color.green);

                if(onLoggedIn != null)
                    onLoggedIn.Invoke();

                playFabId = result.PlayFabId;
                canvas.SetActive(false);
                //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            },
            error =>
            {
                SetDisplayText(error.ErrorMessage, Color.red);
            }
        );
    }

    void SetDisplayText (string text, Color color)
    {
        displayText.text = text;
        displayText.color = color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
