using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TarodevController;


public class DashCooldownUIImageScript : MonoBehaviour
{
    public int flashingRate = 2;
    private PlayerController playerController;

    Image dashImage;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        dashImage = GetComponent<Image>();
        playerController.OnDashFailed += () =>
        {
            StopAllCoroutines();
            StartCoroutine(FlashRed());
        };
    }

    // Update is called once per frame
    void Update()
    {
        var time = Math.Round(playerController.DashCooldown, 2);
        var fill = Mathf.Lerp (0, 1, Mathf.InverseLerp (0, 5, (float)time));
        dashImage.fillAmount = 1 - fill;
    }
    
    IEnumerator FlashRed()
    {
        Debug.Log("Flashing routine started");
        for (int i = 0; i < 100; i += flashingRate)
        {
            dashImage.color = Color.Lerp(Color.white, Color.red, i / 100f);
            yield return null;
        }
        for (int i = 0; i < 100; i += flashingRate)
        {
            dashImage.color = Color.Lerp(Color.red, Color.white, i / 100f);
            yield return null;
        }
        
    }
}
