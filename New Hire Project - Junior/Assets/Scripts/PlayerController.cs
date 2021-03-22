using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using GameSparks;
using TMPro;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed;
    public string gamesparksUserId;

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private float movement; // = to the Vertical axis from the input manager

    public bool player1 = false;
    public bool GameOver = false;

    private GameManager gm;


    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
        gm = FindObjectOfType<GameManager>();
        

        if (this.transform.position.x < 0)
            player1 = true;
        else
            player1 = false;
    }

    void Start()
    {
        if (isLocalPlayer)
        {
            //it is the local player
            GetPlayerInfo();

            //setup
            //Camera.main.transform.SetParent(this.transform); //<-------------------- use if each player needs to move the main camera
        }
        else
        {
            //its not the local player
            rb.isKinematic = true;
        }
    }

    void FixedUpdate()
    { 
        //dont run anything unless it belongs to the local player
        if (!isLocalPlayer)
            return;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S))
            CmdSendMovement();
        else
            CmdSendStop();
    }

    private void Update()
    {
        movement = Input.GetAxisRaw("Vertical");

        if(GameOver && isLocalPlayer)
        {
            //PostScore();
            GameOver = false;
        }
    }

    #region Movement
    [Command]
    void CmdSendMovement()
    {
        //only called on the server
        //validate logic here

        RpcMovePlayer();
    }
    [ClientRpc]
    void RpcMovePlayer()
    {
        rb.velocity = new Vector2(rb.velocity.x, movement * speed);
    }

    [Command]
    void CmdSendStop()
    {
        RpcStop();
    }
    [ClientRpc]
    void RpcStop()
    {
        rb.velocity = new Vector2(0, 0);
    }

    public void Reset()
    {
        rb.velocity = Vector2.zero; //reset the paddles velocity
        transform.position = startPosition; //put the paddle back to the start position
    }
    #endregion

    #region Getting Player ID
    public void GetPlayerInfo()
    {
        new GameSparks.Api.Requests.AccountDetailsRequest().Send((response) => {
            if (!response.HasErrors)
            {
                Debug.Log("Account Details Found...");

                CmdUpdateID(response.UserId);
            }
            else
            {
                Debug.Log("Error Retrieving Account Details...");
            }
        });
    }
    [Command]
    void CmdUpdateID(string id)
    {
        RpcUpdateUserID(id);
    }
    [ClientRpc]
    private void RpcUpdateUserID(string id)
    {
        gamesparksUserId = id;
        Debug.Log("is: " + id);
    }
    #endregion


    public void PostScore(int score)
    {
        if(isLocalPlayer)
        {
            //Debug.Log("you scored: " + score + " Goals");
            //Debug.Log("LOOK HERE"); ////////////////////////////////////////////////<----------------------------------------------

            new GameSparks.Api.Requests.LogEventRequest().SetEventKey("SUBMIT_SCORE").SetEventAttribute("SCORE", score).Send((response) => {
                if (!response.HasErrors)
                {
                    //Debug.Log("Score Posted Successfully...");
                    Debug.Log("you posted: " + score + " Goals to the Leaderboard");
                }
                else
                {
                    Debug.Log("Error Posting Score...");
                }
            });
        }
        

        /*
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
        */
    }
}
