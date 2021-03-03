using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float speed;

    private Rigidbody2D rb;
    private Vector3 startPosition;
    private float movement; // = to the Vertical axis from the input manager

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
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
        
        //input
        movement = Input.GetAxisRaw("Vertical");
        rb.velocity = new Vector2(rb.velocity.x, movement * speed); 
    }

    public void Reset()
    {
        rb.velocity = Vector2.zero; //reset the paddles velocity
        transform.position = startPosition; //put the paddle back to the start position
    }
}
