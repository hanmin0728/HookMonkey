using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundObject : PoolingBase, ISound
{
    [SerializeField] private AudioSource audio;
    public void PlaySound(AudioClip clip, float volume = 1)
    {
        audio.clip = clip;
        audio.volume = volume;
        audio.Play();
    }
}
