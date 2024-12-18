using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lv2_start_player : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] float jumpX = 4f;
    [SerializeField] float jumpY = 4f;
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Jump() {
        rb.AddForce(new Vector2(jumpX, jumpY), ForceMode2D.Impulse);
    }
}
