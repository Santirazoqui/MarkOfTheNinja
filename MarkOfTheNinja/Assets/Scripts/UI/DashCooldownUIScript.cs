using Assets.Scripts.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using TMPro;
using UnityEngine;

public class DashCooldownUIScript : MonoBehaviour
{
    private Color colorWhenReady = Color.yellow;
    public int flashingRate = 10;
    private bool flashing = false;
    private PlayerController player;
    private TextMeshProUGUI text;
    private IEnumerator previusCorutine;
    void Start()
    {
        player = FindAnyObjectByType<PlayerController>();
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        var time = Math.Round(player.DashCooldown, 2);
        text.text = $"Dash cooldown: {Util.ChangeCommaToPoint(time + "")}s";
        FlashText(time);
    }



    private void FlashText(double time)
    {
        if (time == 0 && !flashing)
        {
            
            previusCorutine = FlashingColors();
            StartCoroutine(previusCorutine);
            flashing = true;
        }
        else if (time != 0)
        {
            text.color = Color.white;
            flashing = false;
            if (previusCorutine!=null) StopCoroutine(previusCorutine);
        }
    }

    private IEnumerator FlashingColors()
    {
        while (true)
        {
            for (int i = 0; i < 100; i += flashingRate)
            {
                text.color = Color.Lerp(colorWhenReady, Color.white, i / 100f);
                yield return null;
            }
            for (int i = 0; i < 100; i += flashingRate)
            {
                text.color = Color.Lerp(Color.white, colorWhenReady, i / 100f);
                yield return null;
            }
        }
    }
}
