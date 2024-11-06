using System;
using System.Collections;
using System.Collections.Generic;
using TarodevController;
using TMPro;
using UnityEngine;

public class DashCooldownUIScript : MonoBehaviour
{
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
}
