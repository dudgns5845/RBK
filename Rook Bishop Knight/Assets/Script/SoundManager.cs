using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AudioSource ����Ʈ
/// </summary>
public enum AudioPlayer
{
    Bgm,
    Effect,
}

/// <summary>
/// ����� ����Ʈ
/// </summary>
public enum BgmClips
{
    JazzBruno,
}

/// <summary>
/// ȿ���� ����Ʈ
/// </summary>

public enum EffectClips
{
    BlockBreak,
    ChangePiece,
    UseItem,
    StageClear,
}


public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] List<AudioSource> audios;
    [SerializeField] List<AudioClip> bgmClips;
    [SerializeField] List<AudioClip> effectClips;

    public bool isMute = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// ����� ���
    /// </summary>
    /// <param name="clips"></param>
    public void BGM_Play(BgmClips clips)
    {
        //������� ����ǰ� �ִ��� üũ
        if (audios[(int)AudioPlayer.Bgm].isPlaying) BGM_Stop();
        audios[(int)AudioPlayer.Bgm].PlayOneShot(bgmClips[(int)clips]);
    }

    public void BGM_Stop()
    {
        audios[(int)AudioPlayer.Bgm].Stop();
    }

    /// <summary>
    /// ȿ���� ���
    /// </summary>
    /// <param name="clips"></param>
    public void EFT_Play(EffectClips clips)
    {
        audios[(int)AudioPlayer.Effect].PlayOneShot(effectClips[(int)clips]);
    }


    public void Effect_Stop()
    {
        audios[(int)AudioPlayer.Effect].Stop();
    }

    /// <summary>
    /// ��ü ���Ұ� ���
    /// </summary>
    public void Mute()
    {
        if (isMute == true)
        {
            foreach (AudioSource audio in audios)
            {
                audio.mute = true;
            }
        }
        else
        {
            foreach (AudioSource audio in audios)
            {
                audio.mute = false;
            }
        }
       
    }
}
