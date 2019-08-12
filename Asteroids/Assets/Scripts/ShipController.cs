using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    public float turnVelocity = 10f;
    public float boost = 10f;
    public int health = 3;
    private float turnDirection;
    private float foward;
    public float laserForce = 5f;
    public int laserDamage = 1;
    public float fireRate = 0.1f;
    private float fireCounter;

    public Rigidbody2D m_rigidbody;
    public Transform m_firePoint;
    public GameObject m_laser;

    // Start is called before the first frame update
    void Start()
    {
        if (m_rigidbody == null)
            m_rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        turnDirection = Input.GetAxis("Horizontal");
        foward = Mathf.Abs(Input.GetAxis("Vertical"));
    }

    private void FixedUpdate()
    {
        Die();

        float rotation = (-turnDirection) * turnVelocity * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rotation));

        m_rigidbody.AddForce(transform.up * foward * boost, ForceMode2D.Force);

        Shoot();
    }

    private void Shoot()
    {
        if (fireCounter > 0)
            fireCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Fire1") && fireCounter <= 0)
        {
            fireCounter = fireRate;

            GameObject laser = Instantiate(m_laser, m_firePoint.position, m_firePoint.rotation);
            laser.GetComponent<Laser>().damage = laserDamage;
            laser.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * laserForce * Time.deltaTime, ForceMode2D.Impulse);
            Debug.Log(gameObject.name + " shot!");
        }

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
}
