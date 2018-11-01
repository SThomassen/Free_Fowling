using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParachuteHandler : MonoBehaviour {

    [SerializeField] private Transform m_parachute = null;
    [SerializeField] private float m_speed = 1.0f;

    private Vector3 m_scale = Vector3.zero;
	
	// Update is called once per frame
	void Update () {
        m_parachute.localScale = Vector3.Lerp(m_parachute.localScale, m_scale, m_speed * Time.deltaTime);
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        if (transform.parent != null)
            transform.localRotation = Quaternion.identity;
    }

    public void SetOwner(Transform a_parent)
    {
        transform.parent = a_parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
        transform.localScale = Vector3.one;
    }

    public void OpenParachute()
    {
        m_parachute.gameObject.SetActive(true);
        m_scale = new Vector3(5, 5, 5);
    }
}
