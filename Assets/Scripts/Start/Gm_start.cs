using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gm_start : MonoBehaviour
{
    // 黑色遮罩的 Image 元件
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 2f;
    [SerializeField] string nextScence = "";
    private void Start() {
        fadeImage.gameObject.SetActive(false);
    }
    public void startGame() {
        StartCoroutine(FadeOutAndLoadScene(nextScence));
        fadeImage.gameObject.SetActive(true);
    }

    IEnumerator FadeOutAndLoadScene(string sceneName)
    {
        // 淡出效果：Alpha 從 0（透明）到 1（黑色）
        float timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float alpha = timer / fadeDuration;
            SetImageAlpha(alpha);
            yield return null;
        }
        // 確保完全黑色
        SetImageAlpha(1);

        // 場景切換
        SceneManager.LoadScene(sceneName);
    }

    void SetImageAlpha(float alpha)
    {
        // 設定遮罩圖層的透明度
        Color color = fadeImage.color;
        color.a = Mathf.Clamp01(alpha);
        fadeImage.color = color;
    }
}
