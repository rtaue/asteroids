using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    public float turnVelocity = 10f;
    public float boost = 10f;
    public int health = 3;
    private int lastHealth;
    private float turnDirection;
    private float foward;
    public float laserForce = 5f;
    public int laserDamage = 1;
    public float fireRate = 0.1f;
    private float fireCounter;
    private float invulTime = 0.5f;
    private bool invulnerable = true;

    public Rigidbody2D m_Rigidbody2D;
    public Transform m_FirePoint;
    public Text m_HealthText;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Rigidbody2D == null)
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

        lastHealth = health;
        m_HealthText.text = lastHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        turnDirection = Input.GetAxis("Horizontal");
        foward = Mathf.Abs(Input.GetAxis("Vertical"));

        if (lastHealth != health)
        {
            lastHealth = health;
            m_HealthText.text = lastHealth.ToString();
        }

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

        PlayerMovement();

        Shoot();
    }

    private void PlayerMovement()
    {
        float rotation = (-turnDirection) * turnVelocity * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rotation));

        m_Rigidbody2D.AddForce(transform.up * foward * boost, ForceMode2D.Force);
    }

    private void Shoot()
    {
        if (fireCounter > 0)
            fireCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && fireCounter <= 0)
        {
            fireCounter = fireRate;

            GameObject laser = PoolingManager.instance.GetPooledObject("Player Laser");
            if (laser != null)
            {
                laser.transform.position = m_FirePoint.position;
                laser.transform.rotation = m_FirePoint.rotation;
                laser.SetActive(enabled);
                laser.GetComponent<Laser>().damage = laserDamage;
                laser.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * laserForce * Time.deltaTime, ForceMode2D.Impulse);
            }

            Debug.Log(gameObject.name + " shot!");
        }

    }

    public void Damage(int amount)
    {
        if (!invulnerable)
        {
            if (health > 0)
            {
                health -= amount;
                ResetPosition();
                Debug.Log(gameObject.name + " took damage! -" + amount + " =" + health);
            }
        }
    }

    public void Die()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log(gameObject.name + " destroyed!");
        }

    }

    public void ResetPosition()
    {
        invulnerable = true;
        invulTime = 2f;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
