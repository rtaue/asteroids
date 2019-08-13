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
    private float invulTime = 0.5f;
    private bool invulnerable = true;

    public Rigidbody2D m_Rigidbody2D;
    public GameObject m_MeteorPrefab;

    private void OnEnable()
    {
        if (m_Rigidbody2D == null)
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

        Vector2 force = new Vector2(Random.Range(-minMaxForce, minMaxForce), Random.Range(-minMaxForce, minMaxForce));
        float torque = Random.Range(-minMaxTorque, minMaxTorque);

        m_Rigidbody2D.AddForce(force, ForceMode2D.Force);
        m_Rigidbody2D.AddTorque(torque);
    }

    private void FixedUpdate()
    {
        if (invulTime > 0)
        {
            invulTime -= Time.deltaTime;
        }
        else if (invulTime <= 0)
        {
            if (invulnerable)
                invulnerable = !invulnerable;
        }

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
        if (m_MeteorPrefab != null)
        {
            for (int i = 0; i < 2; i++)
            {
                Instantiate(m_MeteorPrefab, transform.position, Quaternion.identity);
            }
        }

        gameObject.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!invulnerable)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Score();
                collision.gameObject.SendMessage("Damage", damage);
                DestroyMeteor();
                Debug.Log(gameObject.name + " hit: " + collision.gameObject.name);
            }

            if (collision.gameObject.CompareTag("Meteor"))
            {
                DestroyMeteor();
                Debug.Log(gameObject.name + " hit: " + collision.gameObject.name);
            }
        }
    }
}
