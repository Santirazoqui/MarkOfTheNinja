using Assets.Scripts.DataAccess;
using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

public class SpawnPointScript : MonoBehaviour
{
    public Sprite spriteWhenEnabled;
    public Color lightColor;
    // Start is called before the first frame update
    public Transform spawnPosition;
    private BoxCollider2D _collider;
    private AudioSource audioSource;
    private Light2D light;
    private LevelManagerController levelManagerController;
    private float initialLightIntensity;
    private SpriteRenderer spriteRenderer;


    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        light = GetComponentInChildren<Light2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialLightIntensity = light.intensity;
        light.intensity = 0;
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Util.CollidedWithPlayer(collision)) return;
        levelManagerController.CheckpointWasReached(spawnPosition.position);
        _collider.enabled = false;
        audioSource.Play();
        spriteRenderer.sprite = spriteWhenEnabled;
        light.color = lightColor;
        light.intensity = initialLightIntensity;
    }
}
