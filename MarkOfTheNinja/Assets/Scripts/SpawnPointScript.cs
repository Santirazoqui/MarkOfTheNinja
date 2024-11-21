using Assets.Scripts.DataAccess;
using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Zenject;

public class SpawnPointScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform spawnPosition;
    private IDataAccessManager _dataAccessManager;
    private BoxCollider2D _collider;
    private AudioSource audioSource;
    private Light2D light;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
        light = GetComponentInChildren<Light2D>();
    }
    [Inject]
    public void Constructor(IDataAccessManager dataAccessManager)
    {
        _dataAccessManager = dataAccessManager;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!Util.CollidedWithPlayer(collision)) return;
        var currentData = _dataAccessManager.LoadData();
        currentData.SpawnPoint = spawnPosition.position;
        _dataAccessManager.SaveData(currentData);
        _collider.enabled = false;
        audioSource.Play();
        light.color = Color.green;
    }
}
