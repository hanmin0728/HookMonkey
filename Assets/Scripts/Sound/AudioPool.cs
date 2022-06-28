using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPool : PoolAbleObject
{
    public AudioSource source;
    public override void Init_Pop()
    {
        if (source == null)
        {
            source = GetComponent<AudioSource>();
        }
    }

    public override void Init_Push()
    {

    }
    public void PlayAudio(AudioClip clip, float vol = 1f, float pitch = 1f)
    {
        source.clip = clip;
        source.volume = vol;
        source.pitch = pitch;
        source.Play();
        StartCoroutine(WaitForClipLength());
    }
    IEnumerator WaitForClipLength()
    {
        yield return new WaitForSeconds(source.clip.length * 1.1f);
        PoolManager_Test.instance.Push(poolType, gameObject);
    }
}
