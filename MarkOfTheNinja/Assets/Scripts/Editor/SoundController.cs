using Assets.Scripts.Enemy.Pathfinding;
using Assets.Scripts.Enemy.States;
using Assets.Scripts.Sound;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class SoundController : MonoBehaviour
{
    public GameObject soundType;
    public float soundLifeExpectancy = 1f;
    public float radius = 1f;
    private readonly string soundInstanceName = "SoundInstance";

    // Start is called before the first frame update
    void Start()
    {
        LoadSoundPrefab();
    }

    // Update is called once per frame
    void Update()
    {
        CheckForMouseSounds();
    }

    private void CheckForMouseSounds()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CreateSound(mousePosition);
        }
    }

    private void LoadSoundPrefab()
    {
        if (soundType != null) return;
        soundType = Resources.Load<GameObject>(soundInstanceName);
        if (soundType == null) Debug.LogError($"No se encontro el prefab de nombre {soundInstanceName}");
    }    

    private void CreateSound(Vector2 position)
    {
        var sound = Instantiate(soundType, position, Quaternion.identity);
        SoundInstanceController controller = sound.GetComponent<SoundInstanceController>();
        controller.Radius = radius;
        Destroy(sound, soundLifeExpectancy);
    }
}