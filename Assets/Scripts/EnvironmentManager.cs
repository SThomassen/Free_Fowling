using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentManager : MonoBehaviour {

    [SerializeField] private GameObject[] m_platforms = null;
    [SerializeField] private GameObject[] m_obstacles = null;
    [SerializeField] private float m_interval = 1.0f;

    private void Start()
    {
        Invoke("SpawnPlatform", 0);
        Invoke("SpawnObstacle", 0);
    }

    private void SpawnPlatform()
    {
        if (m_platforms.Length == 0) return;
        int type = (int)Random.Range(0, m_platforms.Length -1);
        Instantiate(m_platforms[type], transform.position, Quaternion.identity, transform);
        Invoke("SpawnPlatform", m_interval);
    }

    private void SpawnObstacle() 
    {
        if (m_obstacles.Length == 0) return;
        int type = (int)Random.Range(0, m_obstacles.Length -1);
        Instantiate(m_obstacles[type], transform.position, Quaternion.identity, transform);
        Invoke("SpawnObstacle", m_interval);
    }
}
