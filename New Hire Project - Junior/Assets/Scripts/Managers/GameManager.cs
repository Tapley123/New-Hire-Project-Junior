using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameManager : NetworkBehaviour
{
    #region Variables
    public GameObject player1ScoreText;
    public GameObject player2ScoreText;

    private int player1Score;    
    private int player2Score;
    #endregion

    [ClientRpc]
    public void Player1Scored(int score)
    {
        //player1Score++;
        player1Score = score;
        player1ScoreText.GetComponent<TextMeshProUGUI>().text = player1Score.ToString();
    }

    [ClientRpc]
    public void Player2Scored(int score)
    {
        //player2Score++;
        player2Score = score;
        player2ScoreText.GetComponent<TextMeshProUGUI>().text = player2Score.ToString();
    }
}
