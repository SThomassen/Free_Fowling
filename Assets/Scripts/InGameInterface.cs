using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameInterface : MonoBehaviour {

    [Header("InGame UI")]
    [SerializeField] private Image m_bar = null;
    [SerializeField] private Image m_slider = null;

    [Header("Finish UI")]
    [SerializeField] private Text m_winner = null;
    [SerializeField] private GameObject m_finish = null;
    [SerializeField] private Button m_restart = null;

    [Header("InGame Menu")]
    [SerializeField] private Image m_menu = null;
    [SerializeField] private Button m_back = null;
    [SerializeField] private Image m_disconnect = null;
    [SerializeField] private Text m_controller = null;
    [SerializeField] private Button m_accept = null;

    [Header("Fall Height")]
    [SerializeField] private float m_fallHeight = 100.0f;
    [SerializeField] private float m_fallSpeed = 0.1f;

    private float m_totalFall = 0.0f;
    private float m_totalSize;
    private float m_normalized;

    private bool m_gameFinished = false;

    private string[] m_connected = null;
    private List<int> m_disconnected = new List<int>();

    private void Start()
    {
        Time.timeScale = 1;
        m_totalFall = m_fallHeight;
        m_totalSize = m_bar.rectTransform.rect.height;

        m_normalized = m_totalSize / m_fallHeight;

        string[] connected = Input.GetJoystickNames();
        m_connected = connected;
    }

    private void Update()
    {
        if (m_gameFinished) return;

        if (Input.GetButtonDown("Start_Game"))
        {
            ToggleInGameWindow();
        }

        if (PlayerManager.Instance.IsPaused) return;

        if (m_totalFall <= 0)
        {
            GameFinish();
            m_gameFinished = true;
        }
        else
        {
            m_totalFall -= m_fallSpeed * Time.deltaTime;

            float slidePosition = (m_fallHeight - m_totalFall) * m_normalized;
            m_slider.rectTransform.anchoredPosition = new Vector2(0, -slidePosition);
        }
    }

    public void ToggleInGameWindow()
    {
        bool paused = !m_menu.gameObject.activeSelf;
        m_menu.gameObject.SetActive(paused);
        //Time.timeScale = (paused) ? 1 : 0;
        PlayerManager.Instance.PauseGame(paused);
        m_back.Select();
    }

    public void CloseDisconnectedWindow()
    {
        m_disconnect.gameObject.SetActive(false);

        if (m_disconnected.Count > 0)
        {
            foreach (int i in m_disconnected)
            {
                ControllerData data = ControllerManager.Instance.GetControllerData(i);
                PlayerManager.Instance.RemovePlayer(data.m_playerID);
            }
        }
        Time.timeScale = 1;
    }

    private void GameFinish()
    {
        PlayerManager.Instance.FinishGame();
        Invoke("DisplayFinishWindow", 10);
    }

    private void DisplayFinishWindow()
    {
        m_finish.SetActive(true);
        m_winner.text = string.Format("Player {0}", PlayerManager.Instance.CurrentHolder);
        m_restart.Select();
    }

    private void OnDisconnected(int a_index)
    {
        Debug.LogFormat("controller {0} Disconnected: {1}", a_index, m_connected[a_index]);
        ControllerData data = ControllerManager.Instance.GetControllerData(a_index);
        if (data.m_playerID >= 99) return;

        m_disconnect.gameObject.SetActive(true);
        m_accept.Select();
        m_controller.text = string.Format("Player {0} Disconnected",a_index);
        Time.timeScale = 0;

        m_disconnected.Add(a_index);
    }

    private void OnConnected(int a_index)
    {
        Debug.LogFormat("controller {0} Connected: {1}", a_index, m_connected[a_index]);
        for (int i = 0; i < m_disconnected.Count; i++)
        {
            if (m_disconnected[i] == a_index)
            {
                ControllerData data = ControllerManager.Instance.GetControllerData(m_disconnected[i]);
                PlayerManager.Instance.AddPlayer(data.m_playerID);
                m_disconnected.RemoveAt(i);
            }
        }
    }

    private void FixedUpdate()
    {
        string[] connected = Input.GetJoystickNames();
        for (int i = 0; i < connected.Length; i++)
        {
            if (!connected[i].Equals(m_connected[i]))
            {
                if (connected[i].Equals(string.Empty))
                {
                    OnDisconnected(i);
                    m_connected = connected;
                }
                else
                {
                    m_connected = connected;
                    OnConnected(i);
                }
            }
        }
    }
}
