using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
public class SoundControlBehavior : PlayableBehaviour
{
    public AudioClip _clip;
    public AudioSource _source;

    public override void ProcessFrame(Playable playable, FrameData info, object playerData) //������Ʈ
    {
        //������ ó���߿� �ؾ��Ҳ� �ִٸ� ���⼭
    }
    public override void OnBehaviourPlay(Playable playable, FrameData info) //��ŸƮ
    {
        //�ش� ����� ���۵ɶ� ����
        _source.clip = _clip;
        _source.Play();
    }
}
