using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class VisionConeController : MonoBehaviour
{
    public Vector2 EyesPosition { get => eyes.transform.position; }
    public float visionConeSizeIncreaseWhenDetected = 2;
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
    private LevelManagerController levelManagerController;
    private Light2D light;
    private LightData lightData;
    private PolygonCollider2D polygonCollider;
    private PolygonColliderData polygonColliderData;
    private readonly int pathIndex = 0;
    private bool startFinished = false;
   
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
        SetUpLight();
        SetUpPolygonCollider();
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        levelManagerController.LevelWasReset += OnReset;
        levelManagerController.DetectedModeStarted += IncreaseVisionCone;
        levelManagerController.DetectedModeEnded += ResetVisionConeToOG;
        startFinished = true;
    }

    private void OnReset()
    {
        playerIsBeingSeen = false;
        ResetVisionConeToOG();
    }

    private void SetUpPolygonCollider()
    {
        polygonCollider = GetComponent<PolygonCollider2D>();
        var path = polygonCollider.GetPath(pathIndex);
        polygonColliderData = new()
        {
            Path = new Vector2[path.Length]
        };
        for (int i = 0; i < path.Length; i++)
        {
            polygonColliderData.Path[i] = path[i];
        }
    }

    private void SetUpLight()
    {
        light = GetComponentInChildren<Light2D>();
        lightData = new()
        {
            InnerRadius = light.pointLightInnerRadius,
            OuterRadius = light.pointLightOuterRadius,
            Intensity = light.intensity
        };
    }


    // Update is called once per frame
    void Update()
    {
        if(!startFinished) return;
        UpdatePlayerDetection();
    }

    private void IncreaseVisionCone()
    {
        light.pointLightInnerRadius = lightData.InnerRadius * visionConeSizeIncreaseWhenDetected;
        light.pointLightOuterRadius = lightData.OuterRadius * visionConeSizeIncreaseWhenDetected;
        UpdateCollider(visionConeSizeIncreaseWhenDetected);
    }

    private void ResetVisionConeToOG()
    {
        light.pointLightInnerRadius = lightData.InnerRadius;
        light.pointLightOuterRadius = lightData.OuterRadius;
        UpdateCollider(1);
    }

    private void UpdateCollider(float relativeSizeComparedToOG)
    {
        var newPath = new Vector2[polygonColliderData.Path.Length];
        var center = polygonColliderData.Path[0];
        newPath[0] = center;
        for (int i = 1; i < polygonCollider.points.Length; i++)
        {
            newPath[i] = polygonColliderData.Path[i] + ((polygonColliderData.Path[i]-center)*(relativeSizeComparedToOG-1));
        }
        polygonCollider.SetPath(pathIndex,newPath);
    }

    private void UpdatePlayerDetection()
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


    private struct LightData
    {
        public float InnerRadius;
        public float OuterRadius;
        public float Intensity;
    }

    private struct PolygonColliderData
    {
        public Vector2[] Path;
    }
}
