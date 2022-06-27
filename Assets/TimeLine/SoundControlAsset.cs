using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class SoundControlAsset : PlayableAsset
{
    public ExposedReference<AudioSource> audioSource; //������ ���� �ν����Ϳ��ִ°� �������� ���ص��� �� �������� �����̴ϱ�
    public AudioClip _clip; //���¿��� ���ϴ°� �ӽñ�
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner) //awake������ Ʈ�����ٰ� �̻��⸦ �ø��� �߻��Ǵ°�
    {
        ScriptPlayable<SoundControlBehavior> behaviour = ScriptPlayable<SoundControlBehavior>.Create(graph);

        SoundControlBehavior scb = behaviour.GetBehaviour();

        AudioSource source = audioSource.Resolve(graph.GetResolver());
        scb._source = source;
        scb._clip = _clip;

        return behaviour;
    }
}
