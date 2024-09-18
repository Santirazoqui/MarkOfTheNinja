using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeController : MonoBehaviour
{
    // Start is called before the first frame update
    private readonly string playerTag = "Player";
    private ILevelManager levelManager;
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerWasSeen(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerWasSeen(collision);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void PlayerWasSeen(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(playerTag)) return;
        levelManager = FindAnyObjectByType<LevelManagerController>();
        var enemy = GetComponentInParent<EnemyController>();
        var enemyPosition = enemy.transform.position;
        var playerPosition = collision.gameObject.transform.position;
        float distance = Vector2.Distance(enemyPosition, playerPosition);
        levelManager.PlayerIsBeingSeen(distance);
    }
}
