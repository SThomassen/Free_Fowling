using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour {

    private static PlayerManager m_instance = null;
    public static PlayerManager Instance
    {
        get
        {
            if (m_instance == null)
                m_instance = FindObjectOfType<PlayerManager>();

            return m_instance;
        }
    }

    [Header("Player Data")]
    [SerializeField] private ParachuteHandler m_parachute = null;
    [SerializeField] private PlayerMovement[] m_players = null;
    [SerializeField] private float m_reset = 1;

    private string[] m_controllerNames;
    private int m_previousHolder = -1;
    private int m_currentHolder = -1;

    public int CurrentHolder { get { return m_currentHolder; } }

    private bool m_firstHolder = false;
    private bool m_start = true;

    private bool m_pause = false;
    public bool IsPaused { get { return m_pause; } }

    private void Start()
    {
        if (ControllerManager.Instance == null)
            SceneManager.LoadScene(0);
    }

    private void Update()
    {
        if (m_start && !m_pause)
        {
            transform.position = Vector3.Lerp(transform.position, Vector3.zero, Time.deltaTime);
            m_parachute.transform.position = Vector3.Lerp(m_parachute.transform.position, Vector3.zero, Time.deltaTime);
            if (transform.position.y < 0.2f && m_parachute.transform.position.y > -0.2f)
            {
                transform.position = Vector3.zero;
                m_parachute.transform.position = Vector3.zero;
                m_start = false;
            }
        }
    }

    public void FinishGame()
    {
        foreach (PlayerMovement player in m_players)
        {
            if (player.PlayerID.Equals(m_currentHolder))
            {
                player.SetWinner();
                continue;
            }
            player.SetLoser();
        }

        m_parachute.OpenParachute();
        m_pause = true;
    }

    public void PauseGame(bool a_pause)
    {
        m_pause = a_pause;
        foreach (PlayerMovement player in m_players)
        {
            player.TogglePause(m_pause);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        if (ControllerManager.Instance) ControllerManager.Instance.ResetControllers();
        SceneManager.LoadScene(0);
    }

    public void AddPlayer(int a_player)
    {
        m_players[a_player].gameObject.SetActive(true);
    }

    public void RemovePlayer(int a_player)
    {
        if (m_players[a_player].HasParachute)
        {
            RemoveParachute();
        }

        m_currentHolder = -1;
        m_firstHolder = true;
        m_players[a_player].gameObject.SetActive(false);
    }

    public void RemoveParachute()
    {
        m_parachute.SetOwner(null);
    }

    public void GiveParachute(int a_player, bool a_first)
    {
        if (m_firstHolder != a_first)
        {
            PlayerMovement newPlayer = m_players[a_player - 1];
            m_parachute.SetOwner(newPlayer.Container);

            newPlayer.HasParachute = true;
            m_currentHolder = a_player;
            m_firstHolder = true;
        }
    }

    public void GiveParachute(int a_oldPlayer, int a_newPlayer)
    {
        if (a_newPlayer == m_previousHolder) return;

        PlayerMovement opponent = m_players[a_oldPlayer - 1];
        opponent.HasParachute = false;
        m_previousHolder = a_oldPlayer;

        GiveParachute(a_newPlayer, false);
        Invoke("ResetHolder", m_reset);
    }

    private void ResetHolder()
    {
        m_previousHolder = -1;
    }
}
