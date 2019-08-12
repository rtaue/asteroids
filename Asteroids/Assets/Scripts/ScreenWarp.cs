using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenWarp : MonoBehaviour
{
    public Camera m_camera;
    public BoxCollider2D m_boxCollider2D;

    // Start is called before the first frame update
    void Start()
    {
        m_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 screenSize;
        screenSize.x = Vector2.Distance(m_camera.ScreenToWorldPoint(new Vector2(0, 0)), m_camera.ScreenToWorldPoint(new Vector2(Screen.width, 0)));
        screenSize.y = Vector2.Distance(m_camera.ScreenToWorldPoint(new Vector2(0, 0)), m_camera.ScreenToWorldPoint(new Vector2(0, Screen.height)));
        m_boxCollider2D.size = screenSize;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != null)
        {
            Vector3 viewportPosition = m_camera.WorldToViewportPoint(collision.transform.position);
            Vector3 newPosition = collision.transform.position;

            if (viewportPosition.x > 1 || viewportPosition.x < 0)
            {
                newPosition.x *= -1;
            }

            if (viewportPosition.y > 1 || viewportPosition.y < 0)
            {
                newPosition.y *= -1;
            }

            collision.transform.position = newPosition;

            Debug.Log("Wraping: " + collision.gameObject.name);
        }
    }
}
