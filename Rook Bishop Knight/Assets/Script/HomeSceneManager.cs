using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class HomeSceneManager : MonoBehaviour
{
    //Button 목록
    [SerializeField] Button btn_Start;
    [SerializeField] Button btn_Exit;
    [SerializeField] Button btn_MapEdit;
    [SerializeField] Button btn_GamePlay;
    [SerializeField] Button btn_Audio;


    private void Awake()
    {
        UISetting();
    }

    private void Start()
    {
        SoundManager.instance.BGM_Play(BgmClips.JazzBruno);
    }

    /// <summary>
    /// 최초 씬 실행시 UI 이벤트 등록하는 메소드
    /// </summary>
    void UISetting()
    {
        //버튼에 이벤트 등록
        btn_Start.onClick.AddListener(() => { btn_Start.gameObject.SetActive(false); btn_GamePlay.gameObject.SetActive(true); btn_MapEdit.gameObject.SetActive(true); });
        btn_Exit.onClick.AddListener(() => Application.Quit());
        btn_MapEdit.onClick.AddListener(() => SceneManager.LoadScene("01_Play"));
        btn_GamePlay.onClick.AddListener(() => SceneManager.LoadScene("02_MapEdit"));
        btn_Audio.onClick.AddListener(() =>
        {
            if (SoundManager.instance.isMute == false)
            {
                SoundManager.instance.isMute = true;
                SoundManager.instance.Mute();
                btn_Audio.transform.GetChild(0).gameObject.SetActive(false);
                btn_Audio.transform.GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                SoundManager.instance.isMute = false;
                SoundManager.instance.Mute();
                btn_Audio.transform.GetChild(1).gameObject.SetActive(false);
                btn_Audio.transform.GetChild(0).gameObject.SetActive(true);
            }
        });
    }
}
