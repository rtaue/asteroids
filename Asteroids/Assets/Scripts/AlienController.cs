using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienController : MonoBehaviour
{
    Vector2 newPosition;
    float width;
    float height;

    public float stationaryDuration = 3f;
    private float counter;

    public float spin;

    private Camera m_Camera;
    public Rigidbody2D m_Rigidbody2D;
    public GameObject m_Target;
    public GameObject m_LaserPrefab;

    // Start is called before the first frame update
    void Start()
    {
        if (m_Rigidbody2D == null)
            m_Rigidbody2D = GetComponent<Rigidbody2D>();

        m_Rigidbody2D.AddTorque(spin);

        m_Camera = Camera.main;
        width = Vector2.Distance(m_Camera.ScreenToWorldPoint(new Vector2(0, 0)), m_Camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
        height = Vector2.Distance(m_Camera.ScreenToWorldPoint(new Vector2(0, 0)), m_Camera.ScreenToWorldPoint(new Vector2(0, Screen.height)));

        newPosition = new Vector2(Random.Range((-width / 2), (width / 2)), Random.Range((-height / 2), (height / 2)));
    }

    // Update is called once per frame
    void Update()
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

        if (Input.GetButtonDown("Fire1"))
        {
            Vector3 dir = m_Target.transform.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            Quaternion rot = Quaternion.AngleAxis(angle-90f, Vector3.forward);
            GameObject laser = Instantiate(m_LaserPrefab, transform.position, rot);
            laser.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * 500 * Time.deltaTime, ForceMode2D.Impulse);
        }
    }
}
