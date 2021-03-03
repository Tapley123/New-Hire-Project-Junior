using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Goal : NetworkBehaviour
{
    public bool isPlayer1Goal;
    private GameManager gm;

    private void Awake()
    {
        gm = GameManager.FindObjectOfType<GameManager>();
    }

    /*
    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Ball"))
        {
            if (!isPlayer1Goal)
            {
                Debug.Log("Player 1 scored...");
                //GameObject.Find("Game Manager").GetComponent<GameManager>().Player1Scored();
                gm.Player1Scored();
            }
            else
            {
                Debug.Log("Player 2 scored...");
                //GameObject.Find("Game Manager").GetComponent<GameManager>().Player2Scored();
                gm.Player2Scored();
            }
        }
    }
    */
}
