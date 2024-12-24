using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjRotator_lv2_game : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 360f; // 每秒旋轉度數
    
    private bool isRotating = false;
    private float initialRotation;
    private float currentRotation;

    // 可以自訂的條件檢查（示範用）
    [SerializeField] private float targetTime = 5f; // 目標時間（秒）
    private float elapsedTime = 0f;

    // 開始旋轉的公開方法
    public void StartRotation()
    {
        if (!isRotating)
        {
            // 記錄開始時的角度
            initialRotation = transform.eulerAngles.z;
            StartCoroutine(RotateObject());
        }
    }

    // 檢查條件是否達成（可以根據需求修改）
    private bool CheckCondition()
    {
        elapsedTime += Time.deltaTime;
        return elapsedTime >= targetTime;
    }

    // 處理旋轉的協程
    private IEnumerator RotateObject()
    {
        isRotating = true;
        
        // 持續旋轉直到條件達成
        while (!CheckCondition())
        {
            transform.Rotate(0f, 0f, rotationSpeed * Time.deltaTime);
            yield return null;
        }

        // 計算需要旋轉回原始角度的量
        float currentAngle = transform.eulerAngles.z;
        float targetAngle = initialRotation;

        // 確保我們選擇最短的旋轉路徑
        float angleDifference = Mathf.DeltaAngle(currentAngle, targetAngle);
        
        // 平滑地旋轉回原始角度
        float rotationDuration = 0.5f; // 回轉的持續時間
        float elapsed = 0f;
        
        while (elapsed < rotationDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / rotationDuration;
            
            // 使用 SmoothStep 使動作更流暢
            t = Mathf.SmoothStep(0, 1, t);
            
            float newRotation = currentAngle + (angleDifference * t);
            transform.eulerAngles = new Vector3(0, 0, newRotation);
            
            yield return null;
        }

        // 確保完全回到原始角度
        transform.eulerAngles = new Vector3(0, 0, initialRotation);
        
        isRotating = false;
    }

    // 重設計時器
    public void ResetTimer()
    {
        elapsedTime = 0f;
    }

    // 可選：在Start中自動開始旋轉
    private void Start()
    {
        StartRotation();
    }
}
