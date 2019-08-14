using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
    #region

    public static InputSystem instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.Log("More than one instance of InputSystem found!");
            return;
        }

        instance = this;
    }

    #endregion

    public GameObject m_TouchUI;
    public ShipController m_PlayerController;

    public bool touchInput = false;

    private void Update()
    {
        if (touchInput)
        {
            if (!m_TouchUI.activeSelf)
                m_TouchUI.SetActive(true);
        }
        else
        {
            if (m_TouchUI.activeSelf)
                m_TouchUI.SetActive(false);
        }
    }

    public void Turn(float dir)
    {
        m_PlayerController.SetTurnDirection(dir);
    }

    public void MoveFoward(float foward)
    {
        m_PlayerController.SetFoward(foward);
    }

    public void Shoot(bool fire)
    {
        m_PlayerController.SetFire(fire);
    }
}
