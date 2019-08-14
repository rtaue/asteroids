using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Level
{
    public int meteorQuantity;
    public int alienQuantity;
}

public class LevelManager : MonoBehaviour
{
    #region Singleton

    public static LevelManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of LevelManager found!");
            return;
        }

        instance = this;
    }

    #endregion

    public List<Level> levels;

    public int currentLevel = 1;
    private int level;
    public int count;

    public float startWait;
    public float spawnInterval;
    private float nextLevelWait = 4f;
    private float counter;
    private bool start = false;

    public Text m_LevelText;
    public GameObject m_Player;

    // Start is called before the first frame update
    void Start()
    {
        level = currentLevel - 1;
    }

    private void FixedUpdate()
    {
        if (start)
        {
            if (count <= 0)
            {
                counter += Time.deltaTime;
                if (counter >= nextLevelWait)
                {
                    counter = 0;
                    currentLevel++;
                    if (currentLevel < 12)
                        level++;
                    StartCoroutine(StartLevel(levels[level]));
                }
            }
            else if (counter != 0)
            {
                counter = 0;
            }
        }
    }

    IEnumerator StartLevel(Level level)
    {
        start = false;

        m_LevelText.text = "Level " + currentLevel.ToString("00");

        if (!m_Player.activeSelf)
            m_Player.SetActive(true);
        m_Player.GetComponent<ShipController>().ResetPosition();
        if (m_Player.GetComponent<ShipController>().health < 3)
            m_Player.GetComponent<ShipController>().health = 3;

        yield return new WaitForSeconds(startWait);

        start = true;

        if (level.meteorQuantity > 0)
        {
            Debug.Log("Spawning Meteors");
            for (int i = 0; i < level.meteorQuantity; i++)
            {
                GameObject obj = PoolingManager.instance.GetPooledObject("Meteor Large");
                obj.transform.position = GetMeteorSpawnPosition();
                obj.SetActive(true);
                count++;
            }
        }

        yield return new WaitForSeconds(spawnInterval);

        if (level.alienQuantity > 0)
        {
            Debug.Log("Spawning Aliens");
            for (int i = 0; i < level.alienQuantity; i++)
            {
                GameObject obj;
                if (Random.Range(1,10) > 5)
                    obj = PoolingManager.instance.GetPooledObject("Alien UFO");
                else
                    obj = PoolingManager.instance.GetPooledObject("Alien Ship");

                obj.transform.position = GetAlienSpawnPosition();
                obj.SetActive(true);
                count++;
            }
        }

        yield return null;
        StopAllCoroutines();
    }

    public void StartLevel()
    {
        StopAllCoroutines();
        StartCoroutine(StartLevel(levels[level]));
    }

    private Vector2 GetMeteorSpawnPosition()
    {
        Camera m_Camera = Camera.main;
        float width = Vector2.Distance(m_Camera.ScreenToWorldPoint(new Vector2(0, 0)), m_Camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
        float height = Vector2.Distance(m_Camera.ScreenToWorldPoint(new Vector2(0, 0)), m_Camera.ScreenToWorldPoint(new Vector2(0, Screen.height)));
        float x, y;
        do
        {
            x = Random.Range((-width / 2), (width / 2));
            y = Random.Range((-height / 2), (height / 2));
        } while ((x > -3 && x < 3) || (y > -3 && y < 3));
     
        Vector2 newPosition = new Vector2(x, y);
        return newPosition;
    }

    private Vector2 GetAlienSpawnPosition()
    {
        Camera m_Camera = Camera.main;
        float width = Vector2.Distance(m_Camera.ScreenToWorldPoint(new Vector2(0, 0)), m_Camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
        float height = Vector2.Distance(m_Camera.ScreenToWorldPoint(new Vector2(0, 0)), m_Camera.ScreenToWorldPoint(new Vector2(0, Screen.height)));
        float x, y;
        do
        {
            x = Random.Range((-width / 2 - 3), (width / 2 + 3));
            y = Random.Range((-height / 2 - 3), (height / 2 + 3));
        } while ((x > (-width / 2) && x < (width / 2)) || (y > (-height / 2) && y < (height / 2)));

        Vector2 newPosition = new Vector2(x, y);
        return newPosition;
    }

    public void ResetLevel()
    {
        start = false;
        currentLevel = 1;
        level = currentLevel - 1;
        count = 0;
        m_Player.SetActive(false);

        ScoreManager.instance.currentScore = 0;
        PoolingManager.instance.DisablePooledObjects();
    }

    public void RestartLevel()
    {
        start = false;
        ResetLevel();
        StartLevel();
    }
}
