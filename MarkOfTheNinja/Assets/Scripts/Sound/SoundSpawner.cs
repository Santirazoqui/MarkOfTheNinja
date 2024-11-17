using Assets.Scripts.Sound;
using Assets.Scripts.Util;
using UnityEngine;

public class SoundSpawner : MonoBehaviour
{
    public GameObject soundType;
    public AudioClip entryWaterSound;
    public float soundLifeExpectancy = 0.1f;
    public float soundMaxRadius = 20f;
    public int amountOfIncrements = 60;
    private Vector2 oldPosition;
    private SoundInstanceController soundInstanceController = null;
    private readonly string _audioChildName = "SoundSource";
	private readonly string _playerTag = "Player";
    private AudioSource audioSource;
    private System.Random random;
    // Start is called before the first frame update

    private void Start()
    {
        audioSource= GetComponentInChildren<AudioSource>();
        random = new();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
		SpawnSound(collision);
    }
	
	private void OnCollisionEnter2D(Collision2D collision)
	{
		SpawnSound(collision);
	}

    private void SpawnSound(Collider2D collision)
    {
		if (!Util.CollidedWithPlayer(collision)) return;
        Vector2 playerCenter = collision.bounds.center; // Centro
        PlayEntryWaterSound(playerCenter);
        CreateSound(playerCenter);
    }
	
	private void SpawnSound(Collision2D collision)
    {
		if (!collision.gameObject.CompareTag(_playerTag)) return;
        Vector2 playerCenter = collision.collider.bounds.center; // Centro
        PlayEntryWaterSound(playerCenter);
        CreateSound(playerCenter);
    }

    private void CreateSound(Vector2 position)
    {
        var sound = Instantiate(soundType, position, Quaternion.identity);
        soundInstanceController = sound.GetComponent<SoundInstanceController>();
        soundInstanceController.lifeExpentancy = soundLifeExpectancy;
        soundInstanceController.maxRadius = soundMaxRadius;
        soundInstanceController.amountOfIncrements = amountOfIncrements;
        soundInstanceController.DestructionCallback = OnSoundDestroy;
    }

    private void OnSoundDestroy()
    {
        soundInstanceController = null;
    }

    private void PlayEntryWaterSound(Vector2 position)
    {
        audioSource.gameObject.transform.position = position;
        audioSource.clip = entryWaterSound;
        audioSource.Play();
    }
}
