using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Lv2_player_demo : MonoBehaviour
{
    Rigidbody2D rb;
    private float h_input = 0;
    private bool check = false;
    private float v_speed_tmp = 0;
    [SerializeField] float h_speed = 0, v_speed = 0;
    [SerializeField] GameObject vacm;
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(StartDelay());
    }

    IEnumerator StartDelay() {
        yield return new WaitForSeconds(1f);
        rb.AddForce(new Vector2(4f, 4f), ForceMode2D.Impulse);
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.tag == "trigger_tag") {
            vacm.SetActive(false);
            check = true;
            StartCoroutine(GameDelay());
        }
    }

    IEnumerator GameDelay() {
        yield return new WaitForSeconds(1.5f);
        vacm.SetActive(true);
        v_speed_tmp = v_speed;
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        check = true;
    }

    void FixedUpdate() {
        if (check) {
            h_input = Input.GetAxis("Horizontal");
            rb.velocity = new Vector2(h_input * h_speed, v_speed_tmp);
        }
    }

}
