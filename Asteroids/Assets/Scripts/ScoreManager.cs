using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    #region Singleton

    public static ScoreManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ScoreManager found!");
            return;
        }

        instance = this;
    }

    #endregion

    private int lastScore;
    public int currentScore;

    public Text scoreText;

    private void FixedUpdate()
    {
        if (scoreText != null)
        {
            if (lastScore != currentScore)
            {
                lastScore = currentScore;
                scoreText.text = lastScore.ToString("000000");
            }
        }
    }
}
