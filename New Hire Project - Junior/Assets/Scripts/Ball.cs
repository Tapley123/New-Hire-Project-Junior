using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        Launch();
    }

    void Update()
    {

    }

    private void Launch()
    {
        float x = Random.Range(0, 2) * 2 - 1; //randomly = -1 or 1;
        float y = Random.Range(0, 2) * 2 - 1; //randomly = -1 or 1;

        rb.velocity = new Vector2(speed * x, speed * y);
    }
}
