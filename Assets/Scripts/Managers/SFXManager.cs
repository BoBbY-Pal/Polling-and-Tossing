using System.Collections;
using System.Collections.Generic;
using Singleton;
using UnityEngine;

public class SFXManager : MonoGenericSingleton<SFXManager>
{
    [SerializeField]
    private AudioSource gemBreakSound, explodeSound, stoneBreakSound, roundOverSound;
   
    
    public void PlayGemBreakSound()
    {
        gemBreakSound.Stop();
        gemBreakSound.pitch = Random.Range(.8f, 1.2f);
        gemBreakSound.Play();
    }
    
    public void PlayExplosionSound()
    {
        explodeSound.Stop();
        explodeSound.pitch = Random.Range(.8f, 1.2f);
        explodeSound.Play();
    }
    
    public void PlayStoneBreakSound()
    {
        stoneBreakSound.Stop();
        stoneBreakSound.pitch = Random.Range(.8f, 1.2f);
        stoneBreakSound.Play();
    }
    
    public void PlayRoundOverSound()
    {
        roundOverSound.Play();
    }
}
