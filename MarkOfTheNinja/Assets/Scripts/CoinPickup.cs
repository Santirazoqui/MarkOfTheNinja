using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip[] coinPickupSounds;
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
            int randomIndex = Random.Range(0, coinPickupSounds.Length - 1);
            //Debug.Log("Playing sound " + randomIndex);
            AudioSource.PlayClipAtPoint(coinPickupSounds[randomIndex], Camera.main.transform.position);
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
