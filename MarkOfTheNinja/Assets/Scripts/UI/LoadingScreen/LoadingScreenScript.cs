using Assets.Scripts.Player;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreenScript : MonoBehaviour
{
    private TextMeshPro _textMeshPro;
    private SceneSwitcher sceneSwitcher;

    private void Awake()
    {
        _textMeshPro = GetComponentInChildren<TextMeshPro>();
        sceneSwitcher = GetComponentInParent<SceneSwitcher>();
    }

    private void Update()
    {
        _textMeshPro.text = (sceneSwitcher.CurrentProgress * 100f).ToString("F0") + "%";
    }
}
