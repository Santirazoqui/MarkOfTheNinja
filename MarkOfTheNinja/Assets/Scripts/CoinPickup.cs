using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(AudioSource))]
public class CoinPickup : MonoBehaviour
{
    public AudioClip coinPickupSound;
    //[SerializeField] int pointsForCoinPickup = 100;
	public Sprite defaultSprite;
	public Sprite emptySprite;
    public float timeToRespawn = 2f;
    public float soundVolume = 0.1f; 

    bool wasCollected = false;
	bool wasCollectedBeforeCheckpoint=false;

    private LevelManagerController levelManagerController;
    private SpriteRenderer spriteRenderer;
    private CircleCollider2D collider;
    private Light2D light;
    private void Start()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<CircleCollider2D>();
        light = GetComponentInChildren<Light2D>();
        levelManagerController.CheckpointReached += OnCheckpoint;
        levelManagerController.LevelWasReset += OnReset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Coin collided with " + other.tag);
        if(other.CompareTag("Player"))
        {
            AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position,soundVolume);
            if(!wasCollected)
            {
                levelManagerController.PickedUpCoin();
                spriteRenderer.sprite = emptySprite;
                wasCollected = true;
            }
            StartCoroutine(WaitToRespawn());
        }
    }
	
	private void OnCheckpoint(Vector2 position)
	{
		if(wasCollected)
        {
            wasCollectedBeforeCheckpoint = true;
        }
	}

    private void OnReset()
    {
        if(!wasCollectedBeforeCheckpoint)
        {
            wasCollected = false;
            spriteRenderer.sprite = defaultSprite;
        }
    }

    private IEnumerator WaitToRespawn()
    {
        var transparent = spriteRenderer.color;
        transparent.a = 0;
        spriteRenderer.color = transparent;
        collider.enabled = false;
        light.gameObject.SetActive(false);
        yield return new WaitForSecondsRealtime(timeToRespawn);
        var normal = spriteRenderer.color;
        normal.a = 1;
        spriteRenderer.color = normal;
        spriteRenderer.enabled = true;
        collider.enabled = true;
        light.gameObject.SetActive(true);
    }

}
