using Assets.Scripts.Sound;
using Assets.Scripts.Util;
using UnityEngine;

public class SoundSpawner : MonoBehaviour
{
    public GameObject soundType;
    public AudioClip[] waterSound;
    public AudioClip entryWaterSound;
    private Vector2 oldPosition;
    private SoundInstanceController soundInstanceController = null;
    private readonly string _audioChildName = "SoundSource";
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
        if (!Util.CollidedWithPlayer(collision)) return;
        var position = collision.transform.position;
        PlayEntryWaterSound(position);
        SpawnSound(position);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!Util.CollidedWithPlayer(collision)) return;
        var position = (Vector2)collision.transform.position;
        if(position == oldPosition)
        {
            if(soundInstanceController != null) soundInstanceController.Moving = false;
            return; 
        }
        PlayRandomWaterSound(position);
        SpawnSound(position);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (soundInstanceController != null) soundInstanceController.Moving = false;
    }

    private void SpawnSound(Vector2 position)
    {
        if (soundInstanceController == null)
        {
            CreateSound(position);
        }
        else
        {
            soundInstanceController.Moving = true;
            soundInstanceController.gameObject.transform.position = position;
        }
        oldPosition = position;
    }

    private void CreateSound(Vector2 position)
    {
        var sound = Instantiate(soundType, position, Quaternion.identity);
        soundInstanceController = sound.GetComponent<SoundInstanceController>();
        soundInstanceController.lifeExpentancy = 0.1f;
        soundInstanceController.maxRadius = 12;
        soundInstanceController.amountOfIncrements = 60;
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

    private void PlayRandomWaterSound(Vector2 position)
    {
        if (audioSource.isPlaying) return;
        int randomPos = random.Next(0,waterSound.Length);
        audioSource.clip = waterSound[randomPos];
        audioSource.Play();
    }
}
