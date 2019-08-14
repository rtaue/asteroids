using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class HighScore
{
    public int score;
    public string name;

    public HighScore(int score, string name)
    {
        this.score = score;
        this.name = name;
    }
}

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
    public InputField m_NameInputField;
    public Transform m_HighScoreParent;

    HighScoreSlot[] highScoreSlots;

    public List<HighScore> highScores = new List<HighScore>();

    private void Start()
    {
        highScoreSlots = m_HighScoreParent.GetComponentsInChildren<HighScoreSlot>();
    }

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

    public bool IsHighScore()
    {
        if (highScores.Count > 0)
        {
            if (currentScore > highScores[0].score)
                return true;
            else
                return false;
        }
            return true;
    }

    public void AddScore(string name)
    {
        highScores.Add(new HighScore(currentScore, name));
        highScores.Sort(SortByHighScore);
    }

    public int SortByHighScore(HighScore score1, HighScore score2)
    {
        return score2.score.CompareTo(score1.score);
    }

    public void OnEndEdit()
    {
        m_NameInputField.interactable = false;
        AddScore(m_NameInputField.text);
    }

    public void UpdateHighScoreTop10()
    {
        for (int i = 0; i < 10; i++)
        {
            if (highScores.Count > 0 && i < highScores.Count)
            {
                if (highScores[i] != null)
                {
                    highScoreSlots[i].UpdateHighScore((i + 1), highScores[i].score, highScores[i].name);
                }
                else
                {
                    highScoreSlots[i].UpdateHighScore((i + 1), 000000, "aaa");
                }
            }
            else
            {
                highScoreSlots[i].UpdateHighScore((i + 1), 000000, "aaa");
            }
        }
    }

    public void ClearHighScore()
    {
        highScores.Clear();
    }
}
