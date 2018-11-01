using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerParticleData : MonoBehaviour
{
    PlayerParticleData data;

    //void Awake()
    //{
    //    data = GetComponent<PlayerParticleData>();
    //}
    // Use this for initialization
    void Start()
    {
        data = GetComponent<PlayerParticleData>();
    }

    // Update is called once per frame
    void Update()
    {
      //  if (data != null)
        {
            data.PlayRegular();
        }
    }
}
