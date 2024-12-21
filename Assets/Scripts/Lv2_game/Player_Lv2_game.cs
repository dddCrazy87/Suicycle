using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Lv2_game : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float speed = 1f;
    float h_input = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        h_input = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(h_input * speed, 0);
    }
}
