using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LoginMenuManager : MonoBehaviour
{
    public GameObject loginPanel;
    public GameObject registerPanel;


    void Awake()
    {
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }

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
        GoToLoginScreen();
    }

    //checks the username and password a player has input and if they are correct then the player logs in
    public void B_Login()
    {
        LoadGameScene();
    }
    #endregion
}
