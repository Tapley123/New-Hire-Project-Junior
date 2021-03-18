using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using GameSparks;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed;
    public string gamesparksUserId;

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private float movement; // = to the Vertical axis from the input manager

    public bool player1 = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
        GetPlayerInfo();

        if (this.transform.position.x < 0)
            player1 = true;
        else
            player1 = false;
    }

    void Start()
    {
        if(isLocalPlayer)
        {
            //it is the local player

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
        //Debug.Log("Movement float = " + movement);
        movement = Input.GetAxisRaw("Vertical");
    }

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

    public void GetPlayerInfo()
    {
        new GameSparks.Api.Requests.AccountDetailsRequest().Send((response) => {
            if (!response.HasErrors)
            {
                Debug.Log("Account Details Found...");

                gamesparksUserId = response.UserId; // we can get the display name
                Debug.Log("User Id: " + gamesparksUserId);
            }
            else
            {
                Debug.Log("Error Retrieving Account Details...");
            }
        });
    }
}
