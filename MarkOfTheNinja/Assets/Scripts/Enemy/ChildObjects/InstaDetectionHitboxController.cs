using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstaDetectionHitboxController : MonoBehaviour
{
    private readonly string playerTag = "Player";
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(playerTag)) return;
        var levelManager = FindAnyObjectByType<LevelManagerController>();
        levelManager.PlayerWasInstaDetected();
    }
}
