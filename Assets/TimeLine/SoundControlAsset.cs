using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SoundControlAsset : PlayableAsset
{
    public ExposedReference<AudioSource> audioSource; //정적인 에셋 인스펙터에있는거 프리팹은 안해도됨 왜 프리팹은 에셋이니까
    public AudioClip _clip; //에셋에서 원하는거 머시기
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) //awake같은거 트랙에다가 이새기를 올리면 발생되는거
    {
        ScriptPlayable<SoundControlBehavior> behaviour = ScriptPlayable<SoundControlBehavior>.Create(graph);

        SoundControlBehavior scb = behaviour.GetBehaviour();

        AudioSource source = audioSource.Resolve(graph.GetResolver());
        scb._source = source;
        scb._clip = _clip;

        return behaviour;
    }
}
