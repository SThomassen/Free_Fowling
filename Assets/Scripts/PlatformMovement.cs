using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformMovement : MonoBehaviour
{

    [SerializeField] protected float m_speed = 1.0f;

    // Update is called once per frame
    protected virtual void Update()
    {
        transform.Translate(0, m_speed, 0);

        if (transform.position.y > 50f)
        {
            Destroy(gameObject);
        }
    }
}
