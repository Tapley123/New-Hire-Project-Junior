using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using TMPro;
using Random = UnityEngine.Random;

public class Ball : NetworkBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private NewNetworkManager nm;
    private GameManager gm;
    public float speed;
    public Vector3 startPosition;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
        nm = FindObjectOfType<NewNetworkManager>();
        gm = FindObjectOfType<GameManager>();
    }

    //ball positiion is only simulated on the server in order to syncranise it accross the network
    public override void OnStartServer()
    {
        base.OnStartServer();

        rb.simulated = true; //only simulate ball physics on server

        Launch(); //serve the ball
    }

    
    public void Reset()
    {
        rb.velocity = Vector2.zero; //reset the balls velocity
        transform.position = startPosition; //put the ball back to its start position
        Launch(); //launch the ball again
    }

    private void Launch()
    {
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
                //Debug.Log("Player 1 scored...");
                nm.player1Score++; //increase the score in the network manager
                gm.Player1Scored(nm.player1Score); //have the score be updated for all clients and displayed from the game manager
            }
            else
            {
                //Debug.Log("Player 2 scored...");
                nm.player2Score++; //increase the score in the network manager
                gm.Player2Scored(nm.player2Score); //have the score be updated for all clients and displayed from the game manager
            }

            Reset(); //reset the ball after someone scores
        }
    }
}
