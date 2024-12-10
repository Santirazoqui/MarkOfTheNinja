using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSFX : MonoBehaviour
{
    AudioSource myAudioSource;
    BoxCollider2D myBodyCollider;
    [SerializeField] AudioClip[] jumpSounds;

    int jumpCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        myBodyCollider = GetComponent<BoxCollider2D>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        bool isTouchingGround = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        if (isTouchingGround)
        {
            jumpCounter = 1;
        }
    }

    void OnJump(InputValue value)
    {
        //if (!isAlive) { return; }
        if (value.isPressed && jumpCounter > 0)
        {
            int randomIndex = Random.Range(0, jumpSounds.Length);
            AudioClip randomJumpSound = jumpSounds[randomIndex];
            float volume = 0.2f;
            myAudioSource.PlayOneShot(randomJumpSound, volume);
            jumpCounter--;
        }
    }

}
