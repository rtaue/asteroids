using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor : MonoBehaviour
{
    public float minMaxForce;
    public float minMaxTorque;
    public int health = 1;
    public int damage = 1;

    public Rigidbody2D m_Rigidbody2D;

    // Start is called before the first frame update
    void Start()
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
            Destroy(gameObject);
            Debug.Log(gameObject.name + " destroyed!");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.SendMessage("Damage", damage);
            Destroy(gameObject);
            Debug.Log(gameObject.name + " hit: " + collision.gameObject.name);
        }

        if (collision.gameObject.CompareTag("Meteor"))
        {
            Destroy(gameObject);
            Debug.Log(gameObject.name + " hit: " + collision.gameObject.name);
        }
    }
}
