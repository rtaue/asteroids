using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreSlot : MonoBehaviour
{
    public Text position;
    public Text score;
    public Text playerName;

    public void UpdateHighScore(int position, int score, string name)
    {
        this.position.text = position.ToString("00");
        this.score.text = score.ToString("000000");
        playerName.text = name;
    }
}
