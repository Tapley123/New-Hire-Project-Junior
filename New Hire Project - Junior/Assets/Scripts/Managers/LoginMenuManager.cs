﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginMenuManager : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject registerPanel;

    [Header ("Creating Player")]
    public TMP_InputField displayNameInputCreating;
    public TMP_InputField userNameInputCreating;
    public TMP_InputField passwordInputCreating;

    [Header("Player Login")]
    public TMP_InputField userNameInputLogin;
    public TMP_InputField passwordInputLogin;


    void Awake()
    {
        GoToRegisterScreen();
    }

    void Update()
    {
        //Debug.Log(displayNameInputCreating.text);
    }

    #region Screen / Scene Changers
    private void GoToLoginScreen()
    {
        loginPanel.SetActive(true);
        registerPanel.SetActive(false);
    }
    private void GoToRegisterScreen()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }
    private void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
    #endregion

    #region Buttons
    public void B_GoToLoginScreen()
    {
        GoToLoginScreen();
    }
    public void B_GoToRegisterScreen()
    {
        GoToRegisterScreen();
    }

    //Registers a player on the gamesparks server based on the username and password input
    public void B_CreateAccount()
    {
        if(displayNameInputCreating.text != "" && userNameInputCreating.text != "" && passwordInputCreating.text != "") //check if the input fields are not empty
        {
            Debug.Log("Registering Player...");

            new GameSparks.Api.Requests.RegistrationRequest()
            .SetDisplayName(displayNameInputCreating.text)
            .SetUserName(userNameInputCreating.text)
            .SetPassword(passwordInputCreating.text)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Player Registered \n User Name: " + response.DisplayName);
                    GoToLoginScreen();
                }
                else
                {
                    //display all errors from registering player
                    Debug.Log("Error Registering Player... \n " + response.Errors.JSON.ToString());
                }
            });
        }
        else
            Debug.Log("Error Registering Player... Make sure you have filled in all of the boxes");
    }

    //checks the username and password a player has input and if they are correct then the player logs in
    public void B_Login()
    {
        Debug.Log("Autorising Player...");

        new GameSparks.Api.Requests.AuthenticationRequest()
            .SetUserName(userNameInputLogin.text)
            .SetPassword(passwordInputLogin.text)
            .Send((response) =>
            {
                if (!response.HasErrors)
                {
                    Debug.Log("Player Authenticated \n User Name: " + response.DisplayName);
                    LoadGameScene();
                }
                else
                {
                    //display all errors from registering player
                    Debug.Log("Error Authenticating Player... \n " + response.Errors.JSON.ToString());
                }
            });
    }
    #endregion
}
