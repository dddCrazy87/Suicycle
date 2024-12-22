using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Player_Lv2_game : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] int crushTimes = 0;
    [SerializeField] int[] crushTimesPerLevel;
    int curCrushLv = 0;
    [SerializeField] TextMeshProUGUI crushLvInfo;
    private Rigidbody2D rb;
    private Collider2D cl;
    float h_input = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();
        crushLvInfo.text = crushTimes + " / " + crushTimesPerLevel[curCrushLv];
    }

    void FixedUpdate() {
        h_input = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(h_input * speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Crusher")) {
            print("trigger c");
            if (TriggerCrusher()) {
                cl.enabled = false;
                StartShake(transform);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Obstacle")) {
            print("collision o");
        }
    }

    bool TriggerCrusher() {
        bool isNextLv = false;
        crushTimes++;
        if (crushTimes >= crushTimesPerLevel[curCrushLv]) {
            isNextLv = true;
            if (curCrushLv >= crushTimesPerLevel.Length - 1) {
            // clear
            }
            else {
                crushTimes = 0;
                curCrushLv += 1;
            }
        }
        crushLvInfo.text = crushTimes + " / " + crushTimesPerLevel[curCrushLv];
        return isNextLv;
    }

    [SerializeField] private float shakeDuration = 0.5f;  // 抖動持續時間
    [SerializeField] private float shakeStrength = 0.5f;  // 抖動強度
    [SerializeField] private int shakeVibrato = 10;     // 抖動頻率
    
    public void StartShake(Transform tr) {
        Vector3 pos = tr.position;
        tr.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0, 0), shakeVibrato)
                 .OnComplete(() => {
                    tr.localPosition = pos;
                    cl.enabled = true;
                 })
                 .SetUpdate(true);
    }
}
            