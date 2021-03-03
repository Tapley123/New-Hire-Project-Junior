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
    public float speed;
    public Vector3 startPosition;

    private GameManager gm;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
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

        rb.velocity = new Vector2(speed * x, speed * y);
    }

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            Debug.Log("GOAL");

            if (!collision.GetComponent<Goal>().isPlayer1Goal)
            {
                Debug.Log("Player 1 scored...");
                gm.Player1Scored();
            }
            else
            {
                Debug.Log("Player 2 scored...");
                gm.Player2Scored();
            }
            Reset();
        }
    }
}
