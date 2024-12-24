using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBgManager_lv2_game : MonoBehaviour
{
    [SerializeField] float limitYpos = 7.8f;
    [SerializeField] MoveUp moveUp;
    [SerializeField] Player_Lv2_game player;
    void Update()
    {
        if (transform.position.y > limitYpos) {
            player.EndGame();
            moveUp.StopMoveUp();
        }
    }
}
