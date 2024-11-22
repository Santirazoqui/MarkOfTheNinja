using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoinPickup : MonoBehaviour
{
    public AudioClip coinPickupSound;
    //[SerializeField] int pointsForCoinPickup = 100;
	public Sprite defaultSprite;
	public Sprite emptySprite;

    bool wasCollected = false;
	bool wasCollectedBeforeCheckpoint=false;

    private LevelManagerController levelManagerController;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        levelManagerController.CheckpointReached += OnCheckpoint;
        levelManagerController.LevelWasReset += OnReset;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Coin collided with " + other.tag);
        if(other.CompareTag("Player") && !wasCollected)
        {
            AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
            levelManagerController.PickedUpCoin();
            spriteRenderer.sprite = emptySprite;
            wasCollected = true;
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

}
