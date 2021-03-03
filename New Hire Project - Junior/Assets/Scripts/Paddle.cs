using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : MonoBehaviour
{
    public bool isPlayer1 = false;
    [SerializeField] private float speed;
    private Rigidbody2D rb;
    public Vector3 startPosition;

    private float movement;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    void Update()
    {
        if(isPlayer1)
        {
            movement = Input.GetAxisRaw("Vertical1");
        }
        else
        {
            movement = Input.GetAxisRaw("Vertical2");
        }

        rb.velocity = new Vector2(rb.velocity.x, movement * speed);
    }

    public void Reset()
    {
        rb.velocity = Vector2.zero; //reset the paddles velocity
        transform.position = startPosition; //put the paddle back to the start position
    }
}
