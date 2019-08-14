using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    [Header("Move Properties")]
    public float spin;
    public float stationaryDuration = 3f;
    private float counter;
    private Vector2 newPosition;
    private float width;
    private float height;
    public Rigidbody2D m_Rigidbody2D;

    [Header("Health Properties")]
    public int health = 1;

    [Header("Laser Properties")]
    public float laserForce = 500f;
    public int laserDamage = 1;
    public float fireRate = 2f;
    public float fireCounter;
    public GameObject m_Target;

    [Header("Score Properties")]
    public int maxScore = 200;

    private Camera m_Camera;

    private void OnEnable()
    {
        if (m_Target == null)
            m_Target = GameObject.FindGameObjectWithTag("Player");

        if (m_Rigidbody2D == null)
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

        //Add a spin to Rigidbody2D;
        m_Rigidbody2D.AddTorque(spin);

        //Get screen width and height;
        m_Camera = Camera.main;
        width = Vector2.Distance(m_Camera.ScreenToWorldPoint(new Vector2(0, 0)), m_Camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
        height = Vector2.Distance(m_Camera.ScreenToWorldPoint(new Vector2(0, 0)), m_Camera.ScreenToWorldPoint(new Vector2(0, Screen.height)));

        //Set position to move randomly;
        newPosition = new Vector2(Random.Range((-width / 2), (width / 2)), Random.Range((-height / 2), (height / 2)));

        fireCounter = fireRate;
    }

    private void FixedUpdate()
    {
        Movement();

        Shooting();

        Die();
    }

    //Move UFO to new position, stay for a while and then move to a new position;
    private void Movement()
    {
        Vector2 pos = Vector2.Lerp(transform.position, newPosition, Time.deltaTime);
        transform.position = pos;

        float dist = Vector2.Distance(transform.position, newPosition);
        if (dist < 0.5f)
        {
            counter += Time.deltaTime;
            if (counter > stationaryDuration)
            {
                counter = 0;
                newPosition = new Vector2(Random.Range((-width / 2), (width / 2)), Random.Range((-height / 2), (height / 2)));
            }
        }
    }

    //Shoot Laser to player's position;
    private void Shoot()
    {
        //Get direction to player;
        Vector3 dir = m_Target.transform.position - transform.position;
        //Get rotation's angle to face player;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        //Rotate around z-axis based on angle;
        Quaternion rot = Quaternion.AngleAxis(angle - 90f, Vector3.forward);

        GameObject laser = PoolingManager.instance.GetPooledObject("Alien Laser");
        if (laser != null)
        {
            //Set Laser's positon to UFO position;
            laser.transform.position = transform.position;
            //Set Laser's rotation to face player;
            laser.transform.rotation = rot;
            //Enable Laser, add damage amount and add Relative Force foward to Rigidbody2D;
            laser.SetActive(enabled);
            laser.GetComponent<Laser>().damage = laserDamage;
            laser.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * laserForce * Time.deltaTime, ForceMode2D.Impulse);
        }
    }

    //Shoot based on fire rate;
    private void Shooting()
    {
        fireCounter -= Time.deltaTime;
        if (fireCounter <= 0)
        {
            fireCounter = fireRate;
            Shoot();
        }
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

    //When Health gets to zero disable UFO, decrease LevelManager count and add Score;
    public void Die()
    {
        if (health <= 0)
        {
            Score();
            gameObject.SetActive(false);
            LevelManager.instance.count--;
            Debug.Log(gameObject.name + " destroyed!");
        }

    }

    //Add Score randomly based on maxScore;
    public void Score()
    {
        ScoreManager m_ScoreManager = ScoreManager.instance;
        m_ScoreManager.currentScore += Random.Range(maxScore / 2, maxScore);
    }
}
