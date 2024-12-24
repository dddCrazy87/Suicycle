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

    public GameData gameData;

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

    [SerializeField] AudioSource bgm;
    [SerializeField] AudioClip bgmClip;

    private void Start() {
        bgm.clip = bgmClip;
        bgm.loop = true;
        bgm.Play();
    }
}