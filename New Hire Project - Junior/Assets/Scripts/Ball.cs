using UnityEngine;
using Mirror;
using Random = UnityEngine.Random;


public class Ball : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    private GameManager gm;
    public float speed;
    public Vector3 startPosition;

    private int score1, score2;
    public int AmountToWin = 5;

    bool stopGame = false;

    public PlayerController player1Controller, player2Controller;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
        gm = FindObjectOfType<GameManager>();

        score1 = 0;
        score2 = 0;
    }

    private void Start()
    {
        ResetScore();
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

                    Debug.Log("Player 1's Gamesparks user ID is: " + player1Controller.gamesparksUserId);
                }
                else
                {
                    player2Controller = pc;
                    Debug.Log("Player 2's Gamesparks user ID is: " + player2Controller.gamesparksUserId); //<------------------------------------------------------------shows up as empty!!!!
                }


                new GameSparks.Api.Requests.LeaderboardDataRequest()
                .SetLeaderboardShortCode("SCORE_LEADERBOARD")
                .SetEntryCount(100)
                .Send((response) => {
                    if (!response.HasErrors)
                    {
                        //Debug.Log("Found Leaderboard Data...");

                        foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data)
                        {
                            if (entry.UserId == player1Controller.gamesparksUserId)
                            {
                                string score = entry.JSONData["SCORE"].ToString();
                                string name = entry.UserName;
                                Debug.Log("Player1's name is: " + name + " their score is: " + score);
                            }
                            
                            if (entry.UserId == player2Controller.gamesparksUserId)
                            {
                                string score = entry.JSONData["SCORE"].ToString();
                                string name = entry.UserName;
                                Debug.Log("Player2's name is: " + name + " their score is: " + score);
                            }
                        }
                    }
                    else
                    {
                        //Debug.Log("Error Retrieving Leaderboard Data...");
                    }
                });
            }
        }
    }

    [ServerCallback]
    private void ResetScore()
    {
        gm.UpdatePlayer1Score(score1);
        gm.UpdatePlayer2Score(score2);
    }

    //ball positiion is only simulated on the server in order to syncranise it accross the network
    public override void OnStartServer()
    {
        base.OnStartServer();

        score1 = 0;
        score2 = 0;

        rb.simulated = true; //only simulate ball physics on server

        Launch(); //serve the ball
    }

    public void Reset()
    {
        rb.velocity = Vector2.zero; //reset the balls velocity
        transform.position = startPosition; //put the ball back to its start position

        Launch(); //launch the ball again
    }

    [ClientRpc]
    public void RpcRestartGame()
    {
        stopGame = false;

        score1 = 0;
        score2 = 0;

        rb.simulated = true; //only simulate ball physics on server

        Launch(); //serve the ball
    }

    private void Launch()
    {
        if (stopGame)
            return;

        float x = Random.Range(0, 2) * 2 - 1; //randomly = -1 or 1;
        float y = Random.Range(0, 2) * 2 - 1; //randomly = -1 or 1;

        rb.velocity = new Vector2(speed * x, speed * y); //the ball will be launced in a completely random direction everytime
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            //Debug.Log("GOAL");

            if (!collision.GetComponent<Goal>().isPlayer1Goal)
            {
                score1++;
                gm.UpdatePlayer1Score(score1);

                if (score1 == AmountToWin)
                    Player1Won();
            }
            else
            {
                score2++;
                gm.UpdatePlayer2Score(score2);

                if (score2 == AmountToWin)
                    Player2Won();
            }
            Reset();
        }
    }

    void Player1Won()
    {
        gm.GameWon("Player 1 Won");
        stopGame = true;
        ShowGameSparksIds();
    }

    void Player2Won()
    {
        gm.GameWon("Player 2 Won");
        stopGame = true;
        ShowGameSparksIds();
    }

    void ShowGameSparksIds()
    {
        Debug.Log("Player 1 Gamesparks id: " + player1Controller.gamesparksUserId);
        Debug.Log("Player 2 Gamesparks id: " + player2Controller.gamesparksUserId);
    }

    /*
    public void GetLeaderboard()
    {
        new GameSparks.Api.Requests.LeaderboardDataRequest()
            .SetLeaderboardShortCode("SCORE_LEADERBOARD")
            .SetEntryCount(100)
            .Send((response) => {
                if (!response.HasErrors)
                {
                    //Debug.Log("Found Leaderboard Data...");

                    foreach (GameSparks.Api.Responses.LeaderboardDataResponse._LeaderboardData entry in response.Data)
                    {
                        string score = entry.JSONData["SCORE"].ToString();
                        string playerId = entry.UserId;
                    }
                }
                else
                {
                    //Debug.Log("Error Retrieving Leaderboard Data...");
                }
            });
    }

    public void PostScoreToLeaderboard(int score)
    {
        new GameSparks.Api.Requests.LogEventRequest().SetEventKey("SUBMIT_SCORE").SetEventAttribute("SCORE", score).Send((response) => {
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
    */
    
}
