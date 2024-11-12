using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TarodevController;

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
        PlayerController playerController = player.GetComponent<PlayerController>();
        if(collision.gameObject.tag == "Player"){
            playerController.Active = false;
        }
    }
}
