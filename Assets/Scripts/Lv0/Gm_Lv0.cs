using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gm_Lv0 : MonoBehaviour
{
    [SerializeField] Image backgroundImage;
    [SerializeField] Text dialogueText;
    [SerializeField] Image dialogueChImage;
    [SerializeField] GameObject dialogueUI;
    int curDialogueIndex = 0;
    class Dialogue {
        public string character;
        public string text;
        public string background;
        public string dubbing;
        public string other;
    }
    List<Dialogue> dialogues = new();
    
    void Start() {
        // ---------Fade In---------
        fadeImage.gameObject.SetActive(true);
        StartCoroutine(FadeIn());

        // ---------Set dialogue---------
        if (DialogueManager.Instance != null && DialogueManager.Instance.gameData != null) {
            var dialogueItems = DialogueManager.Instance.gameData.scenes.Find(s => s.sceneName == "Lv0_start").dialogues;
            foreach (var item in dialogueItems) {
                dialogues.Add(new Dialogue { character = item.character, text = item.text , background = item.background, dubbing = item.dubbing, other = item.other });
            }
            ShowDialogue();
        }
    }

    bool isPlayingFadeIn = true;
    private void Update() {
        if (!isPlayingFadeIn) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                NextDialogue();
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                PrevDialogue();
            }
        }
    }

    // ---------Set Dialogue---------
    void ShowDialogue() {
        SetDialogue(dialogues[curDialogueIndex].character, dialogues[curDialogueIndex].text);
        SetBackground(dialogues[curDialogueIndex].background);
    }

    void SetDialogue(string characterName, string textContent) {
        if (textContent == "") {
            dialogueUI.SetActive(false);
            return;
        }
        dialogueUI.SetActive(true);
        Sprite chSprite = Resources.Load<Sprite>("Backgrounds/" + characterName);
        if (chSprite != null) {
            dialogueChImage.sprite = chSprite;
        }
        else {
            print("找不到角色圖片：" + characterName);
        }
        dialogueText.text = textContent;
    }
    
    void SetBackground(string backgroundName)
    {
        Sprite bgSprite = Resources.Load<Sprite>("Backgrounds/" + backgroundName);
        if (bgSprite != null) {
            backgroundImage.sprite = bgSprite;
        }
        else {
            print("找不到背景圖片：" + backgroundName);
        }
    }

    void NextDialogue() {
        if (curDialogueIndex == dialogues.Count - 1) {
            StartCoroutine(FadeOutAndLoadScene(nextScence));
            fadeImage.gameObject.SetActive(true);
        }
        else if (curDialogueIndex >= 0) {
            curDialogueIndex += 1;
            ShowDialogue();
        }
    }

    void PrevDialogue() {
        if (curDialogueIndex == 0) {
            return;
        }
        else if (curDialogueIndex <= dialogues.Count - 1) {
            curDialogueIndex -= 1;
            ShowDialogue();
        }
    }

    // ---------Fade In Fade Out---------
    [SerializeField] Image fadeImage;
    [SerializeField] float fadeDuration = 2.5f;
    [SerializeField] string nextScence = "";
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
        fadeImage.gameObject.SetActive(false);
        isPlayingFadeIn = false;
    }
    IEnumerator FadeOutAndLoadScene(string sceneName) {
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
