using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    //Move Properties;
    public float minMaxForce;
    public float minMaxTorque;
    public Rigidbody2D m_Rigidbody2D;

    //Health Properties;
    public int health = 1;

    //Damage Properties;
    public int damage = 1;

    //Score Properties;
    public int maxScore = 200;

    //Meteor Spawn Properties;
    public string meteorTag = null;
    public bool spawnMeteor = true;
    public int meteorQuantity = 2;
    public bool randomSpawn = false;
    public int maxMeteorQuantity = 3;

    private void OnEnable()
    {
        if (m_Rigidbody2D == null)
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

        //Get a force value randomly based on minMaxForce;
        Vector2 force = new Vector2(Random.Range(-minMaxForce, minMaxForce), Random.Range(-minMaxForce, minMaxForce));
        //Get a torque value randomly based on minMaxTorque;
        float torque = Random.Range(-minMaxTorque, minMaxTorque);

        m_Rigidbody2D.AddForce(force, ForceMode2D.Force);
        m_Rigidbody2D.AddTorque(torque);

        health = 1;
    }

    private void FixedUpdate()
    {
        Die();
    }

    //Decrease Health based on damage amount received;
    public void Damage(int amount)
    {
        if (health > 0)
        {
            health -= amount;
            Debug.Log(gameObject.name + " took damage! -" + amount + " =" + health);
        }
    }

    //When Health gets to zero disable Meteor, decrease LevelManager count, add Score and spawn smaller Meteors;
    public void Die()
    {
        if (health <= 0)
        {
            Score();
            DestroyMeteor();
            Debug.Log(gameObject.name + " destroyed!");
        }

    }

    //Add Score randomly based on maxScore;
    public void Score()
    {
        ScoreManager m_ScoreManager = ScoreManager.instance;
        m_ScoreManager.currentScore += Random.Range(maxScore / 2, maxScore);
    }

    //Disable Meteor, decrease LevelManager count and call SpawnMeteor;
    public void DestroyMeteor()
    {
        SpawnMeteor();
        LevelManager.instance.count--;
        gameObject.SetActive(false);
    }

    //Spawn new Meteors;
    private void SpawnMeteor()
    {
        //Check if it should spawn new Meteors;
        if (spawnMeteor)
        {
            int n;
            //Check if spawn quantity is random or fixed;
            if (randomSpawn)
            {
                n = Random.Range(1, maxMeteorQuantity);
            }
            else
            {
                n = meteorQuantity;
            }

            //Spawn n Meteors;
            for (int i = 0; i < n; i++)
            {
                //Get Meteor from ObjectPooler;
                GameObject obj = PoolingManager.instance.GetPooledObject(meteorTag);
                //Get screen width and height;
                float width = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
                float height = Vector2.Distance(Camera.main.ScreenToWorldPoint(new Vector2(0, 0)), Camera.main.ScreenToWorldPoint(new Vector2(0, Screen.height)));
                //Get Meteor viewport position;
                Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
                //Set new position to Meteor position;
                Vector3 newPosition = transform.position;

                //Check wether new position if off screen or not and adjust new position;
                if (viewportPosition.x > 1)
                    newPosition.x = width / 2;
                else if (viewportPosition.x < 0)
                    newPosition.x = -width / 2;

                if (viewportPosition.y > 1)
                    newPosition.y = height / 2;
                else if (viewportPosition.y < 0)
                    newPosition.y = -height / 2;

                //Set new Meteor spawn position;
                obj.transform.position = newPosition;
                //Enable new Meteor;
                obj.SetActive(true);
                //Add to LevelManager count;
                LevelManager.instance.count++;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Score();
            collision.gameObject.SendMessage("Damage", damage);
            DestroyMeteor();
            Debug.Log(gameObject.name + " hit: " + collision.gameObject.name);
        }
    } 
}
