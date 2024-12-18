using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wall2_Lv2_start : MonoBehaviour
{
    [SerializeField] Lv2_start_gm gm;
    private void OnCollisionEnter2D(Collision2D other) {
        if (other.gameObject.name == "Player") {
            gm.LoadSceneWithFade();
        }
    }
}
