using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TarodevController;


public class DashCooldownUIImageScript : MonoBehaviour
{
    private PlayerController playerController;

    Image dashImage;
    // Start is called before the first frame update
    void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        dashImage = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        var time = Math.Round(playerController.DashCooldown, 2);
        var fill = Mathf.Lerp (0, 1, Mathf.InverseLerp (0, 5, (float)time));
        dashImage.fillAmount = 1 - fill;
    }
}
