using Assets.Scripts.DataAccess;
using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SpawnPointScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Transform spawnPosition;
    private IDataAccessManager _dataAccessManager;
    private BoxCollider2D _collider;
    private AudioSource audioSource;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
        audioSource = GetComponent<AudioSource>();
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
    }
}
