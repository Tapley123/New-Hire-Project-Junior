using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class LeaderboardManager : NetworkBehaviour
{
    public GameObject leaderboardPanel;
    public GameObject leaderboardButton;
    public TMP_Text leaderboardPlayerText;
    public TMP_Text leaderboardScoreText;

    public TMP_InputField scoreInputTest;

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

        new GameSparks.Api.Requests.LeaderboardDataRequest()
            .SetLeaderboardShortCode("SCORE_LEADERBOARD")
            .SetEntryCount(100)
            .Send((response) => {
            if (!response.HasErrors)
            {
                Debug.Log("Found Leaderboard Data...");
                leaderboardPlayerText.text = System.String.Empty;
                leaderboardScoreText.text = System.String.Empty;

                foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data)
                {
                    string score = entry.JSONData["SCORE"].ToString();
                    string playerName = entry.UserName;
                    Debug.Log(" Name:" + playerName + "        Score:" + score + "\n");

                    leaderboardPlayerText.text = leaderboardPlayerText.text + "\n" + playerName;
                    leaderboardScoreText.text = leaderboardScoreText.text + "\n" + score;
                }
            }
            else
            {
                Debug.Log("Error Retrieving Leaderboard Data...");
            }
        });
    }

    public void Button_Back()
    {
        leaderboardPanel.SetActive(false);
        leaderboardButton.SetActive(true);
    }

    public void Button_PostScoreTest()
    {
        new GameSparks.Api.Requests.LogEventRequest().SetEventKey("SUBMIT_SCORE").SetEventAttribute("SCORE", scoreInputTest.text).Send((response) => {
            if (!response.HasErrors)
            {
                Debug.Log("Score Posted Successfully...");
                //Debug.Log("scoreInputTest.text: " + scoreInputTest.text);
            }
            else
            {
                Debug.Log("Error Posting Score...");
            }
        });
    }
}
