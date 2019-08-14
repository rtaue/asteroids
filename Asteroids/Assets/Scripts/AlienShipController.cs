using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShipController : MonoBehaviour
{
    [Header("Move Properties")]
    public float maxSpeed = 5f;
    public float rotSpeed = 90f;
    public float stopDistance = 4f;

    [Header("Health Properties")]
    public int health = 1;

    [Header("Laser Properties")]
    public float laserForce = 5f;
    public float laserRate = 3f;
    private float laserCounter;
    public int laserDamage = 1;
    private bool shoot = false;
    public Transform m_FirePoint;
    public GameObject m_Target;

    [Header("Score Properties")]
    public int maxScore = 200;

    private void OnEnable()
    {
        if (m_Target == null)
            m_Target = GameObject.FindGameObjectWithTag("Player");

        laserCounter = laserRate;
    }

    private void FixedUpdate()
    {
        ShipMovement(m_Target.transform);

        Shooting();

        Die();
    }

    //Rotate Alien Ship to face target;
    private void FaceToTarget (Transform target)
    {
        //Get direction to player;
        Vector3 dir = target.position - transform.position;
        //Get rotation's angle to face player;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        //Get rotation on z-axis based on angle;
        Quaternion desiredRot = Quaternion.Euler(0, 0, angle);
        //Rotate towards desired rotation;
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }

    //Move Alien Ship foward based on maxSpeed;
    private void MoveFoward()
    {
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, maxSpeed * Time.deltaTime, 0);
        pos += transform.rotation * velocity;
        transform.position = pos;
    }

    //Move Alien Ship to Player's position and stop at certain distance;
    private void ShipMovement(Transform target)
    {
        FaceToTarget(target);

        float dist = Vector3.Distance(target.position, transform.position);
        if (dist <= stopDistance)
            return;

        MoveFoward();
    }

    //Shoot at Player when at line of sight based on fire rate;
    private void Shooting()
    {
        if (!shoot)
        {
            laserCounter -= Time.deltaTime;
            if (laserCounter <= 0)
            {
                laserCounter = laserRate;
                shoot = true;
            }
        }

        //Use Raycast to check if Player is on line of sight;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 20f, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            //If Player is on line of sight then shoot;
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            if (shoot)
            {
                shoot = false;
                Shoot();
            }
        }
    }

    //Shoot Laser foward;
    private void Shoot()
    {
        GameObject laser = PoolingManager.instance.GetPooledObject("Alien Laser");
        if (laser != null)
        {
            laser.transform.position = m_FirePoint.position;
            laser.transform.rotation = m_FirePoint.rotation;
            laser.SetActive(enabled);
            laser.GetComponent<Laser>().damage = laserDamage;
            laser.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * laserForce * Time.deltaTime, ForceMode2D.Impulse);

            Debug.Log(gameObject.name + " shot!");
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

    //When Health gets to zero disable Alien Ship, decrease LevelManager count and add Score;
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
