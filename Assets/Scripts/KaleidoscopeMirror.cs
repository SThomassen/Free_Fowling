using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//have to assign shader through code. largely based on Kino.
public class KaleidoscopeMirror : MonoBehaviour
{
    [SerializeField]
    public int _repeat;

    [SerializeField]
    public float _offset;

    [SerializeField]
    public float _roll;

    [SerializeField]
    bool _symmetry;

    [SerializeField] Shader _shader;
    Material _material;
    
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        if (_material == null)
        {
            _material = new Material(_shader);
            _material.hideFlags = HideFlags.DontSave;
        }

        var div = Mathf.PI * 2 / Mathf.Max(1, _repeat);

        _material.SetFloat("_Divisor", div);
        _material.SetFloat("_Offset", _offset * Mathf.Deg2Rad);
        _material.SetFloat("_Roll", _roll * Mathf.Deg2Rad);

        if (_symmetry)
            _material.EnableKeyword("SYMMETRY_ON");
        else
            _material.DisableKeyword("SYMMETRY_ON");

        Graphics.Blit(source, destination, _material);
    }
    
}
