using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSFX : MonoBehaviour
{
    AudioSource myAudioSource;
    //CapsuleCollider2D myBodyCollider;
    [SerializeField] AudioClip[] jumpSounds;

    // Start is called before the first frame update
    void Start()
    {
        //myBodyCollider = GetComponent<CapsuleCollider2D>();
        myAudioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnJump(InputValue value)
    {
        //if (!isAlive) { return; }
        //bool isTouchingGround = myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Ground"));
        /*if(!isTouchingGround) 
        {
            return;
        }*/
        if(value.isPressed)
        {
            int randomIndex = Random.Range(0, jumpSounds.Length);
            AudioClip randomJumpSound = jumpSounds[randomIndex];
            float volume = 0.2f;
            myAudioSource.PlayOneShot(randomJumpSound, volume);
        }
    }

}
