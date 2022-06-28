using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISound
{
    public abstract void PlaySound(AudioClip clip, float volume = 1f);
}
