using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ParticleType
{
    NumberClick,
    WordPlacement,
    WordConnection
}
public class ParticleSystemHandler : MonoBehaviour
{
    public List<ParticleSystem> lsParticle;

    public void PlayParticle(ParticleType type)
    {
        lsParticle[(int)type].Play();
    }
    
}
