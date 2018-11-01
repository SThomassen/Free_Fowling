using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleData : MonoBehaviour
{

    public ParticleSystem regular;
    public ParticleSystem boost;
    public ParticleSystem[] stunned;

    private void Start()
    {
        regular.Stop();
        boost.Stop();
        for (int i = 0; i < stunned.Length; i++)
            stunned[i].Stop();
    }

    public void PlayRegular()
    {
        if (regular == null) return;
        regular.Emit(10);
    }


    public void ResetRegular()
    {
        if (regular == null) return;
        regular.Stop();
    }

    public void PlayBoost()
    {
        if (boost == null) return;
        boost.Emit(30);
    }
    public void ResetBoost()
    {
        if (boost == null) return;
        boost.Stop();
    }

    

    public void PlayStunned()
    {
        for (int i = 0; i < stunned.Length; i++)
            stunned[i].Play();
    }

}
