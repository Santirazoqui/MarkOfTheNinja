using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoinPickup : MonoBehaviour
{
    public AudioClip coinPickupSound;
    //[SerializeField] int pointsForCoinPickup = 100;

    bool wasCollected = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log("Coin collided with " + other.tag);
        if(other.tag == "Player" && !wasCollected)
        {
            wasCollected = true;
            //FindObjectOfType<GameSession>().AddToScore(this.pointsForCoinPickup);
            AudioSource.PlayClipAtPoint(coinPickupSound, Camera.main.transform.position);
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }


}
