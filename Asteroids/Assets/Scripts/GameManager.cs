using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region Singleton

    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of GameManager found!");
            return;
        }

        instance = this;
    }

    #endregion

    public GameObject m_GameOverPanel;
    public GameObject m_PausePanel;

    public Text scoreTitle;
    public Text scoreText;

    public bool isPlaying = false;
    public bool isPaused = false;

    private void Update()
    {
        if (isPlaying)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (isPaused)
                    Continue();
                else
                    Pause();
            }
        }
    }

    public void StartGame()
    {
        isPlaying = true;

        Time.timeScale = 1f;

        LevelManager m_LevelManager = LevelManager.instance;
        if (m_LevelManager)
            m_LevelManager.StartLevel();
    }

    public void RestartGame()
    {
        LevelManager m_LevelManager = LevelManager.instance;
        if (m_LevelManager)
            m_LevelManager.RestartLevel();

        Continue();
    }

    public void GameOver()
    {
        if (!m_GameOverPanel.activeSelf)
            m_GameOverPanel.SetActive(true);

        if (ScoreManager.instance.IsHighScore())
            scoreTitle.text = "high score";
        else
            scoreTitle.text = "score";

        scoreText.text = ScoreManager.instance.currentScore.ToString("000000");

        ScoreManager.instance.m_NameInputField.interactable = true;
        ScoreManager.instance.m_NameInputField.ActivateInputField();

        Time.timeScale = 0f;
        isPlaying = false;
        isPaused = true;
    }

    public void Pause()
    {
        if (!m_PausePanel.activeSelf)
            m_PausePanel.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true; 
    }

    public void Continue()
    {
        if (m_PausePanel.activeSelf)
            m_PausePanel.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isPlaying = false;
        if (m_PausePanel.activeSelf)
            m_PausePanel.SetActive(false);

        LevelManager m_LevelManager = LevelManager.instance;
        if (m_LevelManager)
            m_LevelManager.ResetLevel();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
