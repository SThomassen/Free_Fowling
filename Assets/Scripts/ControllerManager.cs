using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerManager : MonoBehaviour {

    private static ControllerManager m_instance = null;
    public static ControllerManager Instance
    {
        get
        {
            if (m_instance == null)
            {
                m_instance = FindObjectOfType<ControllerManager>();

                if (m_instance != null)
                    DontDestroyOnLoad(m_instance.gameObject);
            }

            return m_instance;
        }
    }

    [SerializeField] private ControllerData[] m_controllers = new ControllerData[8];
    [SerializeField] private ControllerData[] m_slots = new ControllerData[4];

    private int m_activeController = 0;
    public int ActiveControllers { get { return m_activeController; } }

    public bool IsActive(int a_index) { return m_controllers[a_index].m_active; }

    public void AddPlayer(int a_index, string a_type)
    {
        for (int i = 0; i < m_slots.Length; i++)
        {
            if (!m_slots[i].m_active)
            {
                m_slots[i] = new ControllerData(i, a_index, a_type);
                m_controllers[a_index] = new ControllerData(i, a_index, a_type);

                m_activeController++;
                break;
            }
        }
    }

    public void RemovePlayer(int a_index)
    {
        int index = m_controllers[a_index].m_playerID;
        m_controllers[a_index] = new ControllerData();
        m_slots[index] = new ControllerData();

        m_activeController--;
    }

    public ControllerData GetControllerData(int a_controller)
    {
        return m_controllers[a_controller];
    }

    public ControllerData GetPlayerData(int a_player)
    {
        return m_slots[a_player-1];
    }

    public void ResetControllers()
    {
        for (int i = 0; i < m_controllers.Length; i++)
        {
            m_controllers[i] = new ControllerData();
        }

        for (int i = 0; i < m_slots.Length; i++)
        {
            m_slots[i] = new ControllerData();
        }

        m_activeController = 0;
    }

    private void Start()
    {
        m_instance = Instance;
    }
}

[System.Serializable]
public class ControllerData
{
    public enum EModel
    {
        Chicken,
        Kiwi,
        Penguin,
        Kakapo
    }

    public ControllerData()
    {
        m_playerID = 99;
        m_controllerID = 99;
        m_type = string.Empty;
        m_active = false;
    }

    public ControllerData(int a_playerID, int a_controllerID, string a_type)
    {
        m_playerID = a_playerID;
        m_controllerID = a_controllerID;
        m_type = a_type;
        m_active = true;
    }

    public bool m_active = false;
    public int m_controllerID = 99;
    public int m_playerID = 99;
    public string m_type = "J";

    public EModel m_model;
}