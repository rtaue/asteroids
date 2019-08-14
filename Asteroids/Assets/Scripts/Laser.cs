using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public int damage = 1;
    public float duration = 3f;
    private float counter;

    public AudioSource m_AudioSource;
    public AudioClip laserClip;

    // Start is called before the first frame update
    void OnEnable()
    {
        if (m_AudioSource == null)
            m_AudioSource = GetComponent<AudioSource>();

        m_AudioSource.clip = laserClip;

        if (!m_AudioSource.isPlaying)
            m_AudioSource.Play();

        counter = duration;
    }

    private void OnDisable()
    {
        m_AudioSource.Stop();
    }

    private void FixedUpdate()
    {
        counter -= Time.deltaTime;
        if (counter <= 0)
            gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject obj = (collision != null) ? collision.gameObject : null;
        if (obj != null)
        {
            //Debug.Log("Collision detected!");
            if (obj.layer == LayerMask.NameToLayer("Player") 
                || obj.layer == LayerMask.NameToLayer("Enemy") 
                || obj.layer == LayerMask.NameToLayer("Meteor"))
            {
                obj.SendMessage("Damage", damage);
                gameObject.SetActive(false);
                Debug.Log("Hit: " + obj.name);
            }
        } 
    }
}
