using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gm_Lv2_game : MonoBehaviour
{
    // 黑色遮罩的 Image 元件
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 1f;
    
    void Start() {
        if (DialogueManager.Instance != null && DialogueManager.Instance.gameData != null) {
            
        }
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        // 淡入效果：Alpha 從 1（黑色）到 0（透明）
        float timer = fadeDuration;
        while (timer > 0)
        {
            timer -= Time.deltaTime;
            float alpha = timer / fadeDuration;
            SetImageAlpha(alpha);
            yield return null;
        }
        // 確保完全透明
        SetImageAlpha(0);
    }

    void SetImageAlpha(float alpha)
    {
        // 設定遮罩圖層的透明度
        Color color = fadeImage.color;
        color.a = Mathf.Clamp01(alpha);
        fadeImage.color = color;
    }
}
