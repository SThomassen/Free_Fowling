using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

    [Header("Player")]
    [SerializeField] private int m_playerID = 1;
    public int PlayerID { get { return m_playerID; } }

    [Header("Settings")]
    [SerializeField] private float m_speed = 1.0f;
    [SerializeField] private float m_boost = 2.0f;
    [SerializeField] private float m_coolDown = 5.0f;

    [Header("Parachute Container")]
    [SerializeField] private Transform m_container = null;
    public Transform Container { get { return m_container; } }

    private ControllerData m_controller;
    private Rigidbody m_rigidbody = null;
    private Vector3 m_position = Vector3.zero;

    private bool m_hasParachute = false;
    public bool HasParachute
    {
        get { return m_hasParachute; }
        set { m_hasParachute = value; }
    }

    private bool m_winner = false;
    public void SetWinner() { m_winner = true; }
    public void SetLoser()
    {
        m_rigidbody.constraints = RigidbodyConstraints.None;
        m_rigidbody.useGravity = true;
        m_rigidbody.AddForce(0, -20, 0, ForceMode.Impulse);
    }

    private bool m_boosted = false;
    private bool m_collided = false;

    private Vector3 m_velocity = Vector3.zero;
    private PlayerParticleData m_playerParticleData
    {
        get
        {
            if (m_cachedPlayerParticleData == null)
            {
                m_cachedPlayerParticleData = GetComponent<PlayerParticleData>();
            }
            return m_cachedPlayerParticleData;
        }
    }
    private PlayerParticleData m_cachedPlayerParticleData;

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
        m_position = transform.position;
        m_controller = (ControllerManager.Instance) ? ControllerManager.Instance.GetPlayerData(m_playerID) : null;
        if (m_controller == null || m_controller.m_controllerID >= 99)
        {
            gameObject.SetActive(false);
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerManager.Instance.IsPaused && !m_winner) return;
        
        m_position.x = Input.GetAxis(string.Format("{0}{1}_Horizontal", m_controller.m_type,m_controller.m_controllerID)) * m_speed;
        m_position.z = Input.GetAxis(string.Format("{0}{1}_Vertical", m_controller.m_type, m_controller.m_controllerID)) * m_speed;

        float boost = 1;
        if (Input.GetButtonDown(string.Format("{0}{1}_Action", m_controller.m_type, m_controller.m_controllerID)) && !m_boosted)
        {
            boost = m_boost;
            m_boosted = true;
            Invoke("ResetBoost", m_coolDown);
        }

        if (Mathf.Abs(m_position.x) > 0 || Mathf.Abs(m_position.z) > 0)
        {
            Quaternion dir = Quaternion.LookRotation(m_position);
            transform.rotation = Quaternion.Euler(0, dir.eulerAngles.y, 0);
        }
        m_rigidbody.AddForce(m_position * boost, ForceMode.Impulse);

        if (m_boosted)
        {

            m_playerParticleData.PlayBoost();
        } else
        {
            m_playerParticleData.PlayRegular();
        }

    }

    public void TogglePause(bool a_pause)
    {
        if (a_pause)
        {
            m_velocity = m_rigidbody.velocity;
            m_rigidbody.velocity = Vector3.zero;
        }
        else
        {
            m_rigidbody.velocity = m_velocity;
        }
    }

    private void ResetBoost()
    {
        m_playerParticleData.ResetBoost();
        m_playerParticleData.ResetRegular();
        m_playerParticleData.PlayRegular();
        m_boosted = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (PlayerManager.Instance.IsPaused) return;

        switch (other.gameObject.tag)
        {
            case "Parachute":
                PlayerManager.Instance.GiveParachute(m_playerID,true);
                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (PlayerManager.Instance.IsPaused) return;

        switch (other.gameObject.tag)
        {
            case "Player":
                if (m_hasParachute && !m_collided)
                {
                    PlayerMovement opponent = other.gameObject.GetComponent<PlayerMovement>();
                    PlayerManager.Instance.GiveParachute(m_playerID, opponent.PlayerID);
                }
                break;
        }

        m_collided = true;
    }

    private void OnCollisionExit(Collision collision)
    {
        m_collided = false;
    }
}
