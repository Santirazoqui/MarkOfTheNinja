using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionConeController : MonoBehaviour
{
    // Start is called before the first frame update
    private readonly string playerTag = "Player";
    private readonly string groundLayer = "Ground";
    private readonly string wallLayer = "Walls";
    private readonly string visionOriginName = "VisionOrigin";
    private bool playerIsBeingSeen = false;
    private ILevelManager levelManager;
    private float lastDistance;
    private EnemyController enemy;
    private GameObject eyes;
    private void OnTriggerStay2D(Collider2D collision)
    {
        PlayerWasSeen(collision);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerWasSeen(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {

        PlayerLeftVisionRadius(collision);
    }
    void Start()
    {
        Transform eyesTransform = transform.Find(visionOriginName);
        eyes = eyesTransform.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerIsBeingSeen)
        {
            enemy.PlayerIsBeingSeen(lastDistance);
        }
    }

    private void PlayerWasSeen(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(playerTag)) return;
        var enemy = GetComponentInParent<EnemyController>();
        this.enemy = enemy;
        if (ObjectDetector.AnyObjectsBetween(eyes, collision, new string[] {groundLayer,wallLayer})) return;
        var enemyPosition = enemy.transform.position;
        var playerPosition = collision.gameObject.transform.position;
        float distance = Vector2.Distance(enemyPosition, playerPosition);
        lastDistance = distance;
        playerIsBeingSeen = true;
        enemy.PlayerIsBeingSeen(distance);
        
    }

    private void PlayerLeftVisionRadius(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag(playerTag)) return;
        var enemy = GetComponentInParent<EnemyController>();
        if (ObjectDetector.AnyObjectsBetween(enemy.gameObject, collision, new string[] { groundLayer, wallLayer }) && !playerIsBeingSeen) return;
        playerIsBeingSeen = false;
        enemy.PlayerLeftVisionRadius(collision.gameObject.transform.position);
    }
}
