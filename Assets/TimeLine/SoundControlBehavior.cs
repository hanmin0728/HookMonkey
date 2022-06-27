using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class SoundControlBehavior : PlayableBehaviour
{
    public AudioClip _clip;
    public AudioSource _source;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData) //업데이트
    {
        //프레임 처리중에 해야할께 있다면 여기서
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info) //스타트
    {
        //해당 비헤비어가 시작될때 여기
        _source.clip = _clip;
        _source.Play();
    }
}
