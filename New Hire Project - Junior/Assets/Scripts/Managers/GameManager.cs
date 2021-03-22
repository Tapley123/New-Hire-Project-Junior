using UnityEngine;
using TMPro;
using Mirror;

public class GameManager : NetworkBehaviour
{
    #region Variables
    public GameObject player1ScoreText;
    public GameObject player2ScoreText;

    public GameObject winPanel;
    public GameObject winningPlayerName;
    public GameObject replayButton;

    public GameObject leaderboardPanel, leaderboardButton;

    public PlayerController player1Controller, player2Controller;

    #endregion

    private void Awake()
    {
        winPanel.SetActive(false);
    }

    private void Update()
    {
        if (player1Controller == null || player2Controller == null)
        {
            PlayerController[] playerControllers = FindObjectsOfType<PlayerController>();

            foreach (PlayerController pc in playerControllers)
            {
                if (pc.player1)
                {
                    player1Controller = pc;

                    //Debug.Log("Player 1's Gamesparks user ID is: " + player1Controller.gamesparksUserId);
                }
                else
                {
                    player2Controller = pc;
                    //Debug.Log("Player 2's Gamesparks user ID is: " + player2Controller.gamesparksUserId); //<------------------------------------------------------------shows up as empty!!!!
                }
            }
        }
    }


    [ClientRpc]
    public void UpdatePlayer1Score(int score)
    {
        player1ScoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    [ClientRpc]
    public void UpdatePlayer2Score(int score)
    {
        player2ScoreText.GetComponent<TextMeshProUGUI>().text = score.ToString();
    }

    [ClientRpc]
    public void GameWon(string winnerName, int player1Score, int player2Score)
    {
        winPanel.SetActive(true);
        winningPlayerName.GetComponent<TextMeshProUGUI>().text = winnerName;

        player1Controller.PostScore(player1Score);
        player2Controller.PostScore(player2Score);

        if (isServer)
        {
            replayButton.SetActive(true);
        }
        else
        {
            replayButton.SetActive(false);
        }
    }

    public void ReplayButton()
    {
        if (!isServer)
            return;

        RpcRestart();
        FindObjectOfType<Ball>().RpcRestartGame();
    }


    [ClientRpc]
    void RpcRestart()
    {
        player1ScoreText.GetComponent<TextMeshProUGUI>().text = "0";
        player2ScoreText.GetComponent<TextMeshProUGUI>().text = "0";
        winPanel.SetActive(false);
    }

    public void GM_OnStopClient()
    {
        if(!isServer)
        {
            leaderboardPanel.SetActive(false);
            leaderboardButton.SetActive(true);
            winPanel.SetActive(false);
        }
    }
}
