using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBg_Lv2_gm : MonoBehaviour
{
    [SerializeField] BgManager_Lv2_game bgManager;
    // 背景移動的速度
    private float scrollSpeed;
    // 當前背景超出視窗的 Y 位置
    [SerializeField] Transform resetPoint, startPoint;
    private void Start() {
        scrollSpeed = bgManager.scrollSpeed;
    }

    void Update()
    {
        // 持續向上移動
        transform.Translate(Vector3.up * scrollSpeed * Time.deltaTime);

        // 當背景超過重設位置，將它移回初始位置
        if (transform.position.y >= resetPoint.position.y) {
            ResetPosition();
        }
    }

    void ResetPosition() {
        // 移動背景到新的位置
        transform.position = startPoint.position;
    }
}
