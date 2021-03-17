using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LeaderboardManager : NetworkBehaviour
{
    public GameObject leaderboardPanel;
    public GameObject leaderboardButton;

    void Awake()
    {
        leaderboardPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Button_Leaderboard()
    {
        leaderboardPanel.SetActive(true);
        leaderboardButton.SetActive(false);
    }

    public void Button_Back()
    {
        leaderboardPanel.SetActive(false);
        leaderboardButton.SetActive(true);
    }
}
