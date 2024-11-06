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
        text.text = $"Dash cooldown: {ChangeCommaToPoint(time + "")}s";
        if(time == 0 && !flashing)
        {
            StartCoroutine(FlashingColors());
            flashing = true;
        }
    }

    private string ChangeCommaToPoint(string time)
    {
        var res = "";
        for (int i = 0; i < time.Length; i++)
        {
            if (time[i] == ',')
            {
                res += '.';
            }
            else
            {
                res += time[i];
            }
        }
        return res;
    }


    private IEnumerator FlashingColors()
    {
        while (player.DashCooldown == 0)
        {
            for (int i = 0; i < 100 && player.DashCooldown == 0; i += flashingRate)
            {
                text.color = Color.Lerp(colorWhenReady, Color.white, i / 100f);
                yield return null;
            }
            for (int i = 0; i < 100 && player.DashCooldown == 0; i += flashingRate)
            {
                text.color = Color.Lerp(Color.white, colorWhenReady, i / 100f);
                yield return null;
            }
        }
        text.color = Color.white;
        flashing = false;
        yield break;
    }
}
