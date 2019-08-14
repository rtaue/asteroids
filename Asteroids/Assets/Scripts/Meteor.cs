using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float minMaxForce;
    public float minMaxTorque;
    public int health = 1;
    public int damage = 1;
    public int maxScore = 200;
    public string meteorTag = null;
    public bool spawnMeteor = true;
    public int meteorQuantity = 2;
    public bool randomSpawn = false;
    public int maxMeteorQuantity = 3;

    public Rigidbody2D m_Rigidbody2D;

    private void OnEnable()
    {
        if (m_Rigidbody2D == null)
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

        Vector2 force = new Vector2(Random.Range(-minMaxForce, minMaxForce), Random.Range(-minMaxForce, minMaxForce));
        float torque = Random.Range(-minMaxTorque, minMaxTorque);

        m_Rigidbody2D.AddForce(force, ForceMode2D.Force);
        m_Rigidbody2D.AddTorque(torque);

        health = 1;
    }

    private void FixedUpdate()
    {
        Die();
    }

    public void Damage(int amount)
    {
        if (health > 0)
        {
            health -= amount;
            Debug.Log(gameObject.name + " took damage! -" + amount + " =" + health);
        }
    }

    public void Die()
    {
        if (health <= 0)
        {
            Score();
            DestroyMeteor();
            Debug.Log(gameObject.name + " destroyed!");
        }

    }

    public void Score()
    {
        ScoreManager m_ScoreManager = ScoreManager.instance;
        m_ScoreManager.currentScore += Random.Range(maxScore / 2, maxScore);
    }

    public void DestroyMeteor()
    {
        SpawnMeteor();
        LevelManager.instance.count--;
        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Player Laser"))
        {
            Score();
            collision.gameObject.SendMessage("Damage", damage);
            DestroyMeteor();
            Debug.Log(gameObject.name + " hit: " + collision.gameObject.name);
        }
    }

    private void SpawnMeteor()
    {
        if (spawnMeteor)
        {
            int n;
            if (randomSpawn)
            {
                n = Random.Range(1, maxMeteorQuantity);
            }
            else
            {
                n = meteorQuantity;
            }

            for (int i = 0; i < n; i++)
            {
                GameObject obj = PoolingManager.instance.GetPooledObject(meteorTag);
                float width = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
                float height = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)));
                Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
                Vector3 newPosition = transform.position;

                if (viewportPosition.x > 1)
                    newPosition.x = width / 2;
                else if (viewportPosition.x < 0)
                    newPosition.x = -width / 2;

                if (viewportPosition.y > 1)
                    newPosition.y = height / 2;
                else if (viewportPosition.y < 0)
                    newPosition.y = -height / 2;

                obj.transform.position = newPosition;
                obj.SetActive(true);
                LevelManager.instance.count++;
            }
        }
    }
}
