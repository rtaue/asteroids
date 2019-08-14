using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienShipController : MonoBehaviour
{
    public GameObject m_Target;
    public GameObject m_LaserPrefab;
    public Transform m_FirePoint;

    public float maxSpeed = 5f;
    public float rotSpeed = 90f;
    public float stopDistance = 4f;
    public float laserForce = 5f;
    public float laserRate = 3f;
    private float laserCounter;
    public int laserDamage = 1;
    private bool shoot = false;
    public int health = 1;

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

    private void FaceToTarget (Transform target)
    {
        Vector3 dir = target.position - transform.position;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        Quaternion desiredRot = Quaternion.Euler(0, 0, angle);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, desiredRot, rotSpeed * Time.deltaTime);
    }

    private void MoveFoward()
    {
        Vector3 pos = transform.position;
        Vector3 velocity = new Vector3(0, maxSpeed * Time.deltaTime, 0);
        pos += transform.rotation * velocity;
        transform.position = pos;
    }

    private void ShipMovement(Transform target)
    {
        FaceToTarget(target);

        float dist = Vector3.Distance(target.position, transform.position);
        if (dist <= stopDistance)
            return;

        MoveFoward();
    }

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

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, 20f, LayerMask.GetMask("Player"));
        if (hit.collider != null)
        {
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            //Debug.Log(gameObject.name + ": " + hit.collider.gameObject.name + " on sight!");
            if (shoot)
            {
                shoot = false;
                Shoot();
            }
        }
    }

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
            gameObject.SetActive(false);
            LevelManager.instance.count--;
            Debug.Log(gameObject.name + " destroyed!");
        }

    }
}
