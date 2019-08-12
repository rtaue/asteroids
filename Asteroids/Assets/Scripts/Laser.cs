using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public float velocity = 10f;
    public int damage = 1;
    public float duration = 3f;

    public Rigidbody2D m_rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        if (m_rigidbody == null)
            m_rigidbody = GetComponent<Rigidbody2D>();

        //m_rigidbody.AddForce(transform.up * velocity * 100 * Time.deltaTime, ForceMode2D.Impulse);

        Destroy(gameObject, duration);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Meteor"))
        {
            collision.gameObject.SendMessage("Damage", damage);
            Destroy(gameObject);
            Debug.Log("Hit: " + collision.gameObject.name);
        }
    }
}
