using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]

public class Sound
{
    public string soundName;
    public AudioClip clip;
}


public class SoundManager : MonoSingleton<SoundManager>
{
    [Header("���� ���")]
    [SerializeField] Sound[] bgmSounds;

    [SerializeField] Sound[] sfxSounds;

    [Header("��� �÷��̾�")]
    [SerializeField] AudioSource bgmPlayer;

    [Header("ȿ���� �÷��̾�")]
    [SerializeField] AudioSource[] sfxPlayer;//�迭�� �������� ȿ������ ���ÿ� �������� ����� �� �ֱ� ����!


    public void Start()
    {
        PlayRandomBGM();
    }


    //�̰� ���� ������ �������ϴ�

    public void PlaySE(string _soundName)
    {
        for (int i = 0; i < sfxSounds.Length; i++)
        {
            if (_soundName == sfxSounds[i].soundName)
            {//��������� ���� �÷��̾ ã�ƾߵ�
                for (int x = 0; x < sfxPlayer.Length; x++)
                {
                    if (!sfxPlayer[x].isPlaying) //x������ MP3 �÷��̾ ��������� �ʴٸ� �����ϴ� ���ǹ�
                    {
                        //��������� ������
                        sfxPlayer[x].clip = sfxSounds[i].clip;
                        sfxPlayer[x].Play();
                        return; //���ϴ� ȿ������ ã�����Ƿ� return;
                    }
                }
                Debug.Log("��� ȿ���� �÷��̾ ������Դϴ�."); //if���� �ɸ��� �ʾ����Ƿ� ��� MP3 �÷��̾�� ������� ����
                return;
            }
        }
        Debug.Log("��ϵ� ȿ������ ����");
    }
    public void PlayRandomBGM()
    {
        int random = Random.Range(0, bgmSounds.Length);
        //rand = sfxSounds.Length
        bgmPlayer.clip = bgmSounds[random].clip;

        bgmPlayer.Play();

    }

}

