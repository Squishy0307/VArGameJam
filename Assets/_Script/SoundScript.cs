using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundScript : MonoBehaviour
{
    [Header("Audio Stuff")]
    [Space]

    [SerializeField] public AudioSource collectItemSoundEffect;
    [SerializeField] public AudioSource shootItemSoundEffect;
    [SerializeField] public AudioSource hitItemSoundEffect;
    [SerializeField] public AudioSource cherryhitItemSoundEffect;
    [SerializeField] public AudioSource melonExplodeItemSoundEffect;

    public void collectItemSound()
    {
        collectItemSoundEffect.Play();
    }

    public void shootItemSound()
    {
        shootItemSoundEffect.Play();
    }

    public void hitItemSound()
    {
        hitItemSoundEffect.Play();
    }

    public void cherryhitItemSound()
    {
        cherryhitItemSoundEffect.Play();
    }

    public void melonExplodeItemSound()
    {
        melonExplodeItemSoundEffect.Play();
    }

}
