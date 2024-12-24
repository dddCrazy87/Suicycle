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
    // 生成物件
    [SerializeField] ObjectSpawner objectSpawner;
    [SerializeField] float spawnStartDelay = 1.5f;
    [SerializeField] float spawnRepeatRate = 1f;
    
    void Start() {

        if (DialogueManager.Instance != null && DialogueManager.Instance.gameData != null) {
            var dialogueItems = DialogueManager.Instance.gameData.scenes.Find(s => s.sceneName == "Lv2_game").dialogues;
            foreach (var item in dialogueItems) {
                dialogues.Add(new Dialogue { character = item.character, text = item.text , background = item.background, dubbing = item.dubbing, other = item.other });
            }
        }

        StartCoroutine(FadeIn());
        InvokeRepeating("SpawnObj", spawnStartDelay, spawnRepeatRate);
    }
    
    public void StopSpawnObj() {
        CancelInvoke("SpawnObj");
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

    void SpawnObj() {
        objectSpawner.SpawnObjects();
    }


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
    bool isPlayingDialogues = false;
    private void Update() {
        if (isPlayingDialogues) {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
                NextDialogue();
            }
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                PrevDialogue();
            }
        }
    }
    public void ShowDialogue() {
        dialogueUI.SetActive(true);
        isPlayingDialogues = true;
        SetDialogue(dialogues[curDialogueIndex].character, dialogues[curDialogueIndex].text);
    }

    void SetDialogue(string characterName, string textContent) {
        if (textContent == "") {
            dialogueUI.SetActive(false);
            return;
        }
        dialogueUI.SetActive(true);
        Sprite chSprite = Resources.Load<Sprite>("CharacterImgs/" + characterName);
        if (chSprite != null) {
            dialogueChImage.sprite = chSprite;
        }
        else {
            print("找不到角色圖片：" + characterName);
        }
        dialogueText.text = textContent;
    }
    
    [SerializeField] string nextScence = "Final";
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
}
