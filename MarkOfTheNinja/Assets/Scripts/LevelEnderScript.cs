using Assets.Scripts.Enemy;
using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnderScript : MonoBehaviour
{
    // Start is called before the first frame update

    public string goToScreenName = "WinScreen";
    private BoxCollider2D boxCollider;
    private LevelManagerController levelManagerController;
    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(levelManagerController.canWin) {
            if (Util.CollidedWithPlayer(collision)) levelManagerController.PlayerEndedLevel(goToScreenName);
        }
    }
}
