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
    public int curCrushLv = 0, totCrushLv = 5;
    [SerializeField] TextMeshProUGUI crushLvInfo;
    [SerializeField] GameObject[] playerTypePerLevel;
    private Rigidbody2D rb;
    private Collider2D cl;
    private SpriteRenderer spriteRenderer;
    float h_input = 0;
    public bool isInGame = true;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
        cl = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        isInGame = true;
        crushLvInfo.text = crushTimes + " / " + crushTimesPerLevel[curCrushLv];
    }

    void FixedUpdate() {
        if (isInGame) {
            h_input = Input.GetAxis("Horizontal");
            if (h_input > 0) {
                spriteRenderer.flipX = false;
            }
            else if (h_input < 0) {
                spriteRenderer.flipX = true;
            }
            rb.velocity = new Vector2(h_input * speed, 0);
        }
        else {
            rb.velocity = new Vector2(0, -6f);
        }
    }

    public void EndGame() {
        rb.constraints &= ~RigidbodyConstraints2D.FreezePositionY;
        isInGame = false;
        GetComponent<Animator>().SetInteger("type", -1);
    }

    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.CompareTag("Obstacle")) {
            //print("collision o");
        }
    }

    GameObject preCrusher; bool isChangePlayerType = false;
    private void OnTriggerEnter2D(Collider2D other) {
        if (curCrushLv >= totCrushLv - 1) return;
        if (preCrusher == other.gameObject) return;
        if (other.gameObject.CompareTag("Crusher")) {
            preCrusher = other.gameObject;
            if (TriggerCrusher()) {
                isChangePlayerType = true;
            }
            cl.enabled = false;
            StartShake(transform);
            Time.timeScale = 0.4f;
        }
    }

    bool TriggerCrusher() {
        bool isNextLv = false;
        crushTimes++;
        if (crushTimes >= crushTimesPerLevel[curCrushLv]) {
            crushTimes = 0;
            curCrushLv ++;
            isNextLv = true;
        }
        if (curCrushLv >= totCrushLv - 1) {
            crushLvInfo.text = crushTimesPerLevel[curCrushLv - 1] + " / " + crushTimesPerLevel[curCrushLv - 1];
            Invoke("ActiveEndBG", 5f);
        }
        else {
            crushLvInfo.text = crushTimes + " / " + crushTimesPerLevel[curCrushLv];
        }
        return isNextLv;
    }

    [SerializeField] GameObject endBG;
    [SerializeField] gm_Lv2_game gm;
    void ActiveEndBG() {
        gm.StopSpawnObj();
        endBG.SetActive(true);
    }

    [SerializeField] private float shakeDuration = 0.5f;  // 抖動持續時間
    [SerializeField] private float shakeStrength = 0.5f;  // 抖動強度
    [SerializeField] private int shakeVibrato = 10;     // 抖動頻率
    
    void StartShake(Transform tr) {
        Vector3 pos = tr.position;
        tr.DOShakePosition(shakeDuration, new Vector3(shakeStrength, 0, 0), shakeVibrato)
                 .OnComplete(() => {
                    tr.localPosition = pos;
                    cl.enabled = true;
                    Time.timeScale = 1f;
                    if(isChangePlayerType) {
                        ChangePlayerType(playerTypePerLevel[curCrushLv]);
                    }
                    isChangePlayerType = false;
                 })
                 .SetUpdate(true);
    }

    void ChangePlayerType(GameObject sourceObject)
    {
        if (sourceObject != null)
        {
            // 複製 Transform
            Transform sourceTransform = sourceObject.transform;
            transform.rotation = sourceTransform.rotation;
            transform.localScale = sourceTransform.localScale;

            // 複製 Sprite
            SpriteRenderer sourceSpriteRenderer = sourceObject.GetComponent<SpriteRenderer>();
            SpriteRenderer targetSpriteRenderer = GetComponent<SpriteRenderer>();

            if (sourceSpriteRenderer != null && targetSpriteRenderer != null)
            {
                targetSpriteRenderer.sprite = sourceSpriteRenderer.sprite;
            }

            // 複製 CapsuleCollider2D
            CapsuleCollider2D sourceCollider = sourceObject.GetComponent<CapsuleCollider2D>();
            CapsuleCollider2D targetCollider = GetComponent<CapsuleCollider2D>();

            if (sourceCollider != null && targetCollider != null)
            {
                targetCollider.size = sourceCollider.size;
                targetCollider.offset = sourceCollider.offset;
                targetCollider.isTrigger = sourceCollider.isTrigger;
                // 垂直或水平方向
                targetCollider.direction = sourceCollider.direction;
            }

            GetComponent<Animator>().SetInteger("type", curCrushLv);
        }
    }
}
            