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
    #endregion

    private void Awake()
    {
        winPanel.SetActive(false);
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
    public void GameWon(string winnerName)
    {
        winPanel.SetActive(true);
        winningPlayerName.GetComponent<TextMeshProUGUI>().text = winnerName;

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
}
