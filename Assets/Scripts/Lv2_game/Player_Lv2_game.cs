using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Player_Lv2_game : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    public int crushTimes = 0;
    public int crushTimesLv1 = 2,  crushTimesLv2 = 4, crushTimesLv3 = 10;
    int curCrushLv = 1;
    [SerializeField] TextMeshProUGUI crushLvInfo;
    private Rigidbody2D rb;
    float h_input = 0;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        crushLvInfo.text = crushTimes + " / " + crushTimesLv1;
    }

    void FixedUpdate() {
        h_input = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(h_input * speed, 0);
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Crusher")) {
            print("trigger c");
            if (TriggerCrusher()) {
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
        switch (curCrushLv)
        {
            case 1:
                if(crushTimes >= crushTimesLv1) {
                    isNextLv = true;
                    print("hi");
                }
                break;
            case 2:
                if(crushTimes >= crushTimesLv2) {
                    isNextLv = true;
                    print("hi");
                }
                break;
            case 3:
                if(crushTimes >= crushTimesLv3) {
                    isNextLv = true;
                    print("hi");
                }
                break;
            case 4:
                
                break;
            default:
                Debug.LogWarning("crush lv out of limit");
                break;
        }
        return isNextLv;
    }

    [SerializeField] private float shakeDuration = 0.5f;  // 抖動持續時間
    [SerializeField] private float shakeStrength = 0.5f;  // 抖動強度
    [SerializeField] private int shakeVibrato = 10;     // 抖動頻率
    
    public void StartShake(Transform tr) {
        Vector3 pos = tr.position;
        tr.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0, 0), shakeVibrato)
                 .OnComplete(() => tr.localPosition = pos);
    }
}
            