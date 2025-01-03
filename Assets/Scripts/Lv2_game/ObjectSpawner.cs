using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ObjectGroup
    {
        // 此類型的物體預置體
        public List<GameObject> objectPrefabs;
        // 此類型的生成概率
        public float spawnProbability;
    }
    [SerializeField] List<ObjectGroup> objectGroups;
    
    // 生成範圍的左下、右上角
    [SerializeField] Transform spawnAreaMinPoint, spawnAreaMaxPoint;
    Vector2 spawnAreaMin, spawnAreaMax;
    
    // 要生成的物體數量
    [SerializeField] int objectCount = 10;

    // X 軸上的最小、最大間隔
    [System.Serializable]
    public class SpawnXSpacing
    {
        public float minX, maxX, minY, maxY;
    }
    [SerializeField] List<SpawnXSpacing> spawnSpacingPerLevel;
    [SerializeField] Player_Lv2_game player;

    // 記錄已生成物體的位置
    List<Rect> spawnedObjects = new();

    void Start()
    {
        spawnAreaMin = spawnAreaMinPoint.position;
        spawnAreaMax = spawnAreaMaxPoint.position;
    }

    public void SpawnObjects()
    {

        if (player.curCrushLv >= player.totCrushLv - 1) {
            objectGroups[0].spawnProbability = 0;
        }

        // 計算總概率權重
        float totalProbability = CalculateTotalProbability();
        // 初始化生成位置
        float currentX = spawnAreaMin.x;
        float lastY = spawnAreaMin.y;

        for (int i = 0; i < objectCount; i++) {
            // 確保 X 軸間隔在指定範圍內
            float xSpacing = Random.Range(spawnSpacingPerLevel[player.curCrushLv].minX, spawnSpacingPerLevel[player.curCrushLv].maxX);
            currentX += xSpacing;

            // 超出範圍時停止生成
            if (currentX > spawnAreaMax.x) break;

            // 確保 Y 軸間隔在指定範圍內
            float ySpacing = Random.Range(spawnSpacingPerLevel[player.curCrushLv].minY, spawnSpacingPerLevel[player.curCrushLv].maxY);
            float yPosition = Mathf.Clamp(lastY + ySpacing * (Random.value > 0.5f ? 1 : -1), spawnAreaMin.y, spawnAreaMax.y);
            
            // 根據權重選擇物體類型
            GameObject selectedPrefab = SelectRandomPrefab(totalProbability);
            Vector2 spawnPosition = new(currentX, yPosition);

            // 嘗試生成並確保不重疊
            if (!IsOverlapping(spawnPosition, selectedPrefab)) {
                Instantiate(selectedPrefab, spawnPosition, Quaternion.identity);
                Rect objRect = new(spawnPosition, selectedPrefab.GetComponent<SpriteRenderer>().bounds.size);
                spawnedObjects.Add(objRect);
                lastY = yPosition;
            }
        }
        spawnedObjects = new();
    }

    // 計算所有物體類型的總概率
    float CalculateTotalProbability() {
        float total = 0f;
        foreach (var group in objectGroups) {
            total += group.spawnProbability;
        }
        return total;
    }

    // 根據概率權重隨機選擇一個物體預置體
    GameObject SelectRandomPrefab(float totalProbability) {
        float randomPoint = Random.Range(0, totalProbability);
        float currentSum = 0f;

        foreach (var group in objectGroups) {
            currentSum += group.spawnProbability;
            if (randomPoint <= currentSum) {
                // 在當前類型的物體中隨機選擇一個
                return group.objectPrefabs[Random.Range(0, group.objectPrefabs.Count)];
            }
        }

        // 預防萬一，返回第一類型的一個物體
        return objectGroups[0].objectPrefabs[0];
    }

    // 檢查是否與已生成物體重疊
    bool IsOverlapping(Vector2 position, GameObject prefab) {

        Bounds objBounds = prefab.GetComponent<SpriteRenderer>().bounds;
        Rect newRect = new(position, objBounds.size);

        foreach (var rect in spawnedObjects) {
            if (newRect.Overlaps(rect)) {
                return true;
            }
        }
        return false;
    }
}
