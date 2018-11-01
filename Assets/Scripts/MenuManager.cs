using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour {

    private enum EMode
    {
        Title,
        Select
    }
    private EMode m_mode = EMode.Title;

    [SerializeField] private GameObject m_title = null;
    [SerializeField] private GameObject m_selection = null;
    [SerializeField] private Image[] m_keyboards = null;
    [SerializeField] private Image[] m_joysticks = null;
    [SerializeField] private Text[] m_texts = null;
    [SerializeField] private Text m_start = null;

    private void Update()
    {
        int length = ControllerManager.Instance.ActiveControllers;

        switch(m_mode)
        {
            case EMode.Title:
                if (Input.GetButtonDown("Start_Game"))
                {
                    m_title.SetActive(false);
                    m_selection.SetActive(true);
                    m_mode = EMode.Select;
                }
                break;
            case EMode.Select:
                //Start Game
                if (Input.GetButtonDown("Start_Game") && length > 1)
                {
                    SceneManager.LoadScene(1);
                }

                if (length < 4)
                {
                    AddJoystick();
                    AddKeyboard();
                }

                if (length > 0)
                {
                    RemoveJoystick();
                    RemoveKeyboard();
                }
                break;
        }
        
        #region Editor Specifics
#if UNITY_EDITOR
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
    #endif
#endregion
    }

    private void AddJoystick()
    {
        //Join Joystick
        if (Input.GetButtonDown("J0_Action") && !ControllerManager.Instance.IsActive(0))
        {
            ControllerManager.Instance.AddPlayer(0, "J");
            DisplayProfile(0,true);
        }
        if (Input.GetButtonDown("J1_Action") && !ControllerManager.Instance.IsActive(1))
        {
            ControllerManager.Instance.AddPlayer(1, "J");
            DisplayProfile(1, true);
        }
        if (Input.GetButtonDown("J2_Action") && !ControllerManager.Instance.IsActive(2))
        {
            ControllerManager.Instance.AddPlayer(2, "J");
            DisplayProfile(2, true);
        }
        if (Input.GetButtonDown("J3_Action") && !ControllerManager.Instance.IsActive(3))
        {
            ControllerManager.Instance.AddPlayer(3, "J");
            DisplayProfile(3, true);
        }
        if (Input.GetButtonDown("J4_Action") && !ControllerManager.Instance.IsActive(4))
        {
            ControllerManager.Instance.AddPlayer(4, "J");
            DisplayProfile(4, true);
        }
    }

    private void AddKeyboard()
    {
        //Join Keyboard
        if (Input.GetButtonDown("K5_Action") && !ControllerManager.Instance.IsActive(5))
        {
            ControllerManager.Instance.AddPlayer(5, "K");
            DisplayProfile(5, false);
        }
        if (Input.GetButtonDown("K6_Action") && !ControllerManager.Instance.IsActive(6))
        {
            ControllerManager.Instance.AddPlayer(6, "K");
            DisplayProfile(6, false);
        }
    }

    private void RemoveJoystick()
    {
        if (Input.GetButtonDown("J0_Back") && ControllerManager.Instance.IsActive(0))
        {
            HideProfile(0);
            ControllerManager.Instance.RemovePlayer(0);
        }
        if (Input.GetButtonDown("J1_Back") && ControllerManager.Instance.IsActive(1))
        {
            HideProfile(1);
            ControllerManager.Instance.RemovePlayer(1);
        }
        if (Input.GetButtonDown("J2_Back") && ControllerManager.Instance.IsActive(2))
        {
            HideProfile(2);
            ControllerManager.Instance.RemovePlayer(2);
        }
        if (Input.GetButtonDown("J3_Back") && ControllerManager.Instance.IsActive(3))
        {
            HideProfile(3);
            ControllerManager.Instance.RemovePlayer(3);
        }
        if (Input.GetButtonDown("J4_Back") && ControllerManager.Instance.IsActive(4))
        {
            HideProfile(4);
            ControllerManager.Instance.RemovePlayer(4);
        }
    }

    private void RemoveKeyboard()
    {
        if (Input.GetButtonDown("K5_Back") && ControllerManager.Instance.IsActive(5))
        {
            HideProfile(5);
            ControllerManager.Instance.RemovePlayer(5);
        }
        if (Input.GetButtonDown("K6_Back") && ControllerManager.Instance.IsActive(6))
        {
            HideProfile(6);
            ControllerManager.Instance.RemovePlayer(6);
        }
    }

    private void DisplayProfile(int a_player, bool a_controller)
    {
        int index = ControllerManager.Instance.ActiveControllers;
        ControllerData data = ControllerManager.Instance.GetControllerData(a_player);

        m_keyboards[data.m_playerID].gameObject.SetActive(!a_controller);
        m_joysticks[data.m_playerID].gameObject.SetActive(a_controller);
        m_texts[data.m_playerID].gameObject.SetActive(false);

        if (index > 1)
            m_start.gameObject.SetActive(true);
    }

    private void HideProfile(int a_player)
    {
        int index = ControllerManager.Instance.ActiveControllers;
        ControllerData data = ControllerManager.Instance.GetControllerData(a_player);

        m_keyboards[data.m_playerID].gameObject.SetActive(false);
        m_joysticks[data.m_playerID].gameObject.SetActive(false);
        m_texts[data.m_playerID].gameObject.SetActive(true);

        if (index <= 2) // Player amount has not been reduced yet.
            m_start.gameObject.SetActive(false);
    }
}
