using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BgManager_Lv2_game : MonoBehaviour
{
    Vector2 startPos;
    float repeatHeight = 10f;

    void Start() {
        startPos = transform.position;
    }

    void Update()
    {
        if (transform.position.y > repeatHeight) {
            transform.position = startPos;
        }
    }
}
