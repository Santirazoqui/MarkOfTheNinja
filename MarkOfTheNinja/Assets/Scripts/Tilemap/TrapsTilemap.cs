using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;
using Assets.Scripts.Player;

public class TrapsTilemap : MonoBehaviour
{
    LevelManagerController levelManagerController;
    GameObject player;
    
    void Start()
    {
        levelManagerController = FindObjectOfType<LevelManagerController>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Player"){
            PlayerController playerController = player.GetComponent<PlayerController>();
            playerController.Explode();
        }
    }
}
