using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveUp : MonoBehaviour
{
    public float speeda = 6f;
    bool isBackground = false, isPause = false;
    private void Start() {
        if (gameObject.CompareTag("Background")) {
            isBackground = true;
        }
    }
    void Update()
    {
        if (!isPause) {
            transform.Translate(Vector3.up * speeda * Time.deltaTime);
        }
        if (!isBackground && transform.position.y > 6.5f) {
            Destroy(gameObject);
        }
    }

    public void StopMoveUp() {
        isPause = true;
    }
}
