using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleMovement : PlatformMovement
{
    [Range(1f, 20f)]
    public float randomizationFactor;

    [SerializeField] private Vector3 spawnOffset;
    
    void Start()
    {
        Vector3 pos = Random.insideUnitSphere * randomizationFactor;
        pos += spawnOffset;
       // pos.y = pos.y - pos.y;
        transform.position = pos;

    }
    protected override void Update()
    {
        base.Update();
    }

    private void OnCollisionExit(Collision other)
    {

        if (other.gameObject.name.Contains("Player") && gameObject.name.Contains("Spike"))//nasty but meh.
        {
            AudioHelper audio = gameObject.GetComponent<AudioHelper>();
            if (audio)
            {
                audio.Play();
            }
            else
            {
                audio = gameObject.GetComponentInChildren<AudioHelper>();
                if (audio)
                {
                    audio.Play();
                }
            }
        }
       

    }

}
