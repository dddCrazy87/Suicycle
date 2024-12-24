using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgManager_Lv2_game : MonoBehaviour
{
    Vector2 startPos;
    float repeatHeight = 10.15f;

    void Start() {
        startPos = transform.position;
        //repeatHeight = transform.GetChild(0).GetComponent<BoxCollider2D>().size.y*2;
        //print(repeatHeight);
    }

    void Update()
    {
        if (transform.position.y > repeatHeight) {
            transform.position = startPos;
        }
    }
}
