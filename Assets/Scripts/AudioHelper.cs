using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHelper : MonoBehaviour
{
    private AudioSource m_audioSource
    {
        get
        {
            if (m_cachedaudioSource == null)
            {
                m_cachedaudioSource = GetComponent<AudioSource>();
            }
            return m_cachedaudioSource;
        }
    }
    private AudioSource m_cachedaudioSource;
    // Use this for initialization
    [SerializeField]
    private AudioClip[] clips;
    public void Play()
    {
        for (int i = 0; i < clips.Length; i++)
        {
            if (clips.Length == 1)
            {
                PlayFromIndex(0);
                break;
            }
            else
            {
                PlayFromIndex((int)Random.Range(0f, clips.Length));
            }

        }
    }
    private void PlayFromIndex(int a_index)
    {
        m_audioSource.clip = clips[a_index];
        if (m_audioSource.clip != null)
        {
            m_audioSource.PlayOneShot(clips[a_index]);
        }
    }
}
