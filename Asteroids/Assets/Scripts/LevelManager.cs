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
    private int lastLevel;
    private int level;
    public int count;

    public float startWait;
    public float spawnInterval;
    private float nextLevelWait = 2f;
    private float counter;

    public Text m_LevelText;
    public GameObject m_Player;

    // Start is called before the first frame update
    void Start()
    {
        lastLevel = currentLevel;
        level = currentLevel - 1;
        m_LevelText.text = "Level " + currentLevel.ToString("00");

        StartCoroutine(StartLevel(levels[level]));
    }

    private void FixedUpdate()
    {
        if (lastLevel != currentLevel)
        {
            lastLevel = currentLevel;
            m_LevelText.text = "Level " + currentLevel.ToString("00");
        }

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

    IEnumerator StartLevel(Level level)
    {
        m_Player.GetComponent<ShipController>().ResetPosition();
        if (m_Player.GetComponent<ShipController>().health < 3)
            m_Player.GetComponent<ShipController>().health = 3;

        yield return new WaitForSeconds(startWait);

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
}
