using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 1;
    public float duration = 3f;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, duration);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Meteor"))
        {
            collision.gameObject.SendMessage("Damage", damage);
            Destroy(gameObject);
            Debug.Log("Hit: " + collision.gameObject.name);
        }
    }
}
