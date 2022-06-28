using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour, ISound
{
    [SerializeField] private GameObject soundObj;
    public void PlaySound(AudioClip clip, float volume = 1)
    {
        Instantiate(soundObj).GetComponent<SoundObject>().PlaySound(clip, volume);
    }
    public void Sound(AudioClip _clip)
    {
        PlaySound(_clip);
    }
}