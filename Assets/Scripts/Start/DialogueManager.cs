using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [System.Serializable]
    public class Dialogue
    {
        public string character;
        public string text;
        public string background;
        public string dubbing;
        public string other;
    }

    [System.Serializable]
    public class SceneData
    {
        public string sceneName;
        public List<Dialogue> dialogues;
    }

    [System.Serializable]
    public class GameData
    {
        public List<SceneData> scenes;
    }

    public Image backgroundImage;
    public Text characterNameText;
    public Text dialogueText;
    public Text otherInfoText;

    public AudioSource audioSource;

    public GameData gameData;
    private int currentDialogueIndex;
    private SceneData currentSceneData;

    void Awake()
    {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadDialogueData();
        }
        else {
            Destroy(gameObject);
        }
    }

    void Start() {
        
        //SceneManager.sceneLoaded += OnSceneLoaded;

        // foreach (var item in gameData.scenes) {
        //     print(item.sceneName + ":");
        //     foreach (var item2 in item.dialogues) {
        //         print(item2.background);
        //         print(item2.character + " : " + item2.text);
        //         print("dubbing: " + item2.dubbing);
        //         print("other: " + item2.other);
        //     }
        //     print("---");
        // }
    }

    void LoadDialogueData() {
        string filePath = Path.Combine(Application.streamingAssetsPath, "dialogue.json");
        if (File.Exists(filePath)) {
            string jsonData = File.ReadAllText(filePath);
            gameData = JsonUtility.FromJson<GameData>(jsonData);
        }
        else {
            Debug.LogError("找不到 JSON 檔案：" + filePath);
        }
    }

    // void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     currentSceneData = gameData.scenes.Find(s => s.sceneName == scene.name);
    //     if (currentSceneData != null) {
    //         print("這個場景有對話：" + scene.name);
    //         currentDialogueIndex = 0;
    //         //ShowDialogue();
    //     }
    //     else {
    //         print("這個場景沒有對話：" + scene.name);
    //     }
    // }


    void ShowDialogue()
    {
        if (currentSceneData != null && currentDialogueIndex < currentSceneData.dialogues.Count)
        {
            Dialogue currentDialogue = currentSceneData.dialogues[currentDialogueIndex];

            // 顯示角色名稱和對話
            characterNameText.text = currentDialogue.character;
            dialogueText.text = currentDialogue.text;

            // 顯示其他資訊
            if (otherInfoText != null) {
                otherInfoText.text = currentDialogue.other;
            }

            // 切換背景圖片
            if (!string.IsNullOrEmpty(currentDialogue.background)) {
                SetBackground(currentDialogue.background);
            }

            // 播放配音
            if (!string.IsNullOrEmpty(currentDialogue.dubbing)) {
                PlayDubbing(currentDialogue.dubbing);
            }
        }
        else {
            Debug.Log("場景對話播放完畢！");
        }
    }

    public void OnClickNextDialogue()
    {
        currentDialogueIndex++;
        ShowDialogue();
    }

    void SetBackground(string backgroundName)
    {
        Sprite bgSprite = Resources.Load<Sprite>("Backgrounds/" + backgroundName);
        if (bgSprite != null) {
            backgroundImage.sprite = bgSprite;
        }
        else {
            Debug.LogError("找不到背景圖片：" + backgroundName);
        }
    }

    void PlayDubbing(string dubbingName)
    {
        AudioClip clip = Resources.Load<AudioClip>("Audio/" + dubbingName);
        if (clip != null) {
            audioSource.clip = clip;
            audioSource.Play();
        }
        else {
            Debug.LogError("找不到配音檔案：" + dubbingName);
        }
    }
}