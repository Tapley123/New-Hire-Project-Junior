using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Mirror;

public class GameManager : NetworkBehaviour
{
    #region Variables
    [Header("Ball")]
    public GameObject ball;

    [Header("Player 1")]
    public GameObject player1Paddle;
    public GameObject player1Goal;

    [Header("Player 2")]
    public GameObject player2Paddle;
    public GameObject player2Goal;

    [Header("Score UI")]
    public GameObject player1ScoreText;
    public GameObject player2ScoreText;

    private int player1Score;    
    public int player2Score;
    #endregion


    public void Player1Scored()
    {
        player1Score++;
        player1ScoreText.GetComponent<TextMeshProUGUI>().text = player1Score.ToString();
        //ResetPosition();
    }

    

    public void Player2Scored()
    {
        player2Score++;
        player2ScoreText.GetComponent<TextMeshProUGUI>().text = player2Score.ToString();
        //ResetPosition();
    }

    /*
    private void ResetPosition()
    {
        ball.GetComponent<Ball>().Reset();
        player1Paddle.GetComponent<Paddle>().Reset();
        player2Paddle.GetComponent<Paddle>().Reset();
    }
    */

    [Command]
    public void CmdScoreUp(int score)
    {
        // Server say all clients, your score
        RpcScoreUp(score);
    }

    [ClientRpc]
    public void RpcScoreUp(int score)
    {
        // You dont need do this action again, will be do it only your instance on all clients
        if (!isLocalPlayer)
        {
            //myScore = score;
            //scorePlayer1.text = score.ToString();
        }
    }
}
