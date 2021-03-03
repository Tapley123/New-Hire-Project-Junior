using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
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
    public static TextMeshProUGUI Player1Score;
    public GameObject player2ScoreText;
    public static TextMeshProUGUI Player2Score;

    public bool StartGame = false;

    private int player1Score;
    private int player2Score;
    #endregion

    private void Update()
    {
        Player1Score = player1ScoreText.GetComponent<TextMeshProUGUI>();
        Player2Score = player2ScoreText.GetComponent<TextMeshProUGUI>();
    }

    public void Player1Scored()
    {
        player1Score++;
        player1ScoreText.GetComponent<TextMeshProUGUI>().text = player1Score.ToString();
        ResetPosition();
    }

    public void Player2Scored()
    {
        player2Score++;
        player2ScoreText.GetComponent<TextMeshProUGUI>().text = player2Score.ToString();
        ResetPosition();
    }

    private void ResetPosition()
    {
        ball.GetComponent<Ball>().Reset();
        player1Paddle.GetComponent<Paddle>().Reset();
        player2Paddle.GetComponent<Paddle>().Reset();
    }

    public void Begin()
    {
        StartGame = true;
    }
}
