using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreScript : MonoBehaviour
{
    [Header("Increase/Descrease score animation options")]
    public Color defaultColor = Color.white;
    public Color loosingPointsColor = Color.red;
    public Color gainingPointsColor = Color.green;
    public float secondsUntilScoreUpdates = 0.5f;
    public int visibleIncreases = 30;


    private LevelManagerController levelManagerController;
    private int? previousScore;
    private TextMeshProUGUI text;
    void Start()
    {
        levelManagerController = FindAnyObjectByType<LevelManagerController>();
        text = GetComponent<TextMeshProUGUI>();
        previousScore = levelManagerController.Score;
        ShowScore(levelManagerController.Score);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateText();
    }

    private void UpdateText()
    {
        if (levelManagerController.Score == previousScore) return;
        if (levelManagerController.Score > previousScore)
        {
            StartCoroutine(PlayIncreasingScoreAnimation(levelManagerController.Score, (int)previousScore));
            previousScore = levelManagerController.Score;
            return;
        }
        StartCoroutine(PlayDecreasingScoreAnimation(levelManagerController.Score, (int)previousScore));
        previousScore = levelManagerController.Score;

        
    }

    private IEnumerator PlayIncreasingScoreAnimation(int currentScore, int previousScore)
    {
        float increase = (currentScore - previousScore) /visibleIncreases;
        text.color = gainingPointsColor;
        int currentShownScore = -1;
        for (float i=previousScore; i<=currentScore; i+= increase)
        {
            currentShownScore = (int)i;
            ShowScore(currentShownScore);
            yield return new WaitForSeconds(secondsUntilScoreUpdates/visibleIncreases);
        }
        text.color = defaultColor;
        if (currentShownScore != currentScore) ShowScore(currentScore);
        yield break;
    }

    private IEnumerator PlayDecreasingScoreAnimation(int currentScore, int previousScore)
    {
        float decrease = (previousScore - currentScore) / visibleIncreases;
        text.color = loosingPointsColor;
        int currentShownScore = -1;
        for (float i = previousScore; i >= currentScore; i -= decrease)
        {
            currentShownScore = (int)i;
            ShowScore(currentShownScore);
            yield return new WaitForSeconds(secondsUntilScoreUpdates / visibleIncreases);
        }
        text.color = defaultColor;
        if (currentShownScore != currentScore) ShowScore(currentScore);
        yield break;
    }

    private void ShowScore(int score)
    {
        text.text = $"Score: {score}";
    }
}
