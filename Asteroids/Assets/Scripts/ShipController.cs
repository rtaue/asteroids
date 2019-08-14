using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShipController : MonoBehaviour
{
    [Header("Move Properties")]
    public float turnVelocity = 10f;
    public float boost = 10f;
    private float turnDirection;
    private float foward;
    public Rigidbody2D m_Rigidbody2D;

    [Header("Health Properties")]
    public int health = 3;
    private int lastHealth;
    public Text m_HealthText;

    [Header("Laser Properties")]
    public float laserForce = 5f;
    public int laserDamage = 1;
    public float fireRate = 0.1f;
    private float fireCounter;
    private bool fire;
    public Transform m_FirePoint;

    private float invulTime = 0.5f;
    private bool invulnerable = true;


    private void OnEnable()
    {
        if (m_Rigidbody2D == null)
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

        lastHealth = health;
        m_HealthText.text = lastHealth.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();

        SetHealthUI();
    }

    private void FixedUpdate()
    {
        Invulnerable();

        Die();

        PlayerMovement();

        Shoot();
    }

    //Rotate ship around the z-axis and add force foward to the Rigidbody2D;
    private void PlayerMovement()
    {
        float rotation = (-turnDirection) * turnVelocity * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rotation));

        m_Rigidbody2D.AddForce(transform.up * foward * boost, ForceMode2D.Force);
    }

    //Get a Laser Object from the Object Pooler, position it in the Fire Point position and rotatation,
    //then add a Relative Force to Laser's Rigidbody2D foward, shoot based on fire rate;
    public void Shoot()
    {
        if (fireCounter > 0)
            fireCounter -= Time.deltaTime;

        if (fire && fireCounter <= 0)
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
    
    //Decrease Health based on damage amount received if not invulnerable;
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

    //When Health gets to zero disable Player Ship and call GameOver panel;
    public void Die()
    {
        if (health <= 0)
        {
            gameObject.SetActive(false);
            GameManager.instance.GameOver();
            Debug.Log(gameObject.name + " destroyed!");
        }

    }

    //Set Player Ship position to start position (screen center);
    public void ResetPosition()
    {
        invulnerable = true;
        invulTime = 2f;
        transform.position = Vector2.zero;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    public void GetInput()
    {
        if (!InputSystem.instance.touchInput)
        {
            turnDirection = Input.GetAxis("Horizontal");
            foward = Mathf.Abs(Input.GetAxis("Vertical"));
            fire = Input.GetButtonDown("Fire1");
        }
    }

    public void SetTurnDirection(float direction)
    {
        turnDirection = direction;
    }

    public void SetFoward(float foward)
    {
        this.foward = foward;
    }

    public void SetFire(bool fire)
    {
        this.fire = fire;
    }

    public void SetHealthUI()
    {
        if (lastHealth != health)
        {
            lastHealth = health;
            m_HealthText.text = lastHealth.ToString();
        }
    }

    public void Invulnerable()
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
    }
}
