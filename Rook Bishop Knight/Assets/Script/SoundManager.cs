using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AudioSource 리스트
/// </summary>
public enum AudioPlayer
{
    Bgm,
    Effect,
}

/// <summary>
/// 배경음 리스트
/// </summary>
public enum BgmClips
{
    JazzBruno,
}

/// <summary>
/// 효과음 리스트
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
    /// 배경음 재생
    /// </summary>
    /// <param name="clips"></param>
    public void BGM_Play(BgmClips clips)
    {
        //배경음이 실행되고 있는지 체크
        if (audios[(int)AudioPlayer.Bgm].isPlaying) BGM_Stop();
        audios[(int)AudioPlayer.Bgm].PlayOneShot(bgmClips[(int)clips]);
    }

    public void BGM_Stop()
    {
        audios[(int)AudioPlayer.Bgm].Stop();
    }

    /// <summary>
    /// 효과음 재생
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
    /// 전체 음소거 기능
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
