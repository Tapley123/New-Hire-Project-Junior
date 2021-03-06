using UnityEngine;
using TMPro;
using Mirror;

public class GameManager : NetworkBehaviour
{
    #region Variables
    public GameObject player1ScoreText;
    public GameObject player2ScoreText;
    #endregion

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
}
