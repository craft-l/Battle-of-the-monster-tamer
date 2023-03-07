using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 控制音乐、音效播放
/// </summary>
public class AudioSourceManager : BaseManager
{

    private AudioSource[] audioSource;//0.播放BGMusic 1.播放特效音
    private bool playEffectMusic = true;
    private bool playBGMusic = true;

    public AudioSourceManager()
    {
        audioSource = GameManager.Instance.GetComponents<AudioSource>();
    }

    //播放背景音乐
    public void PlayBGMusic(AudioClip audioClip)
    {
        if (!audioSource[0].isPlaying || audioSource[0].clip != audioClip)
        {
            audioSource[0].clip = audioClip;
            audioSource[0].Play();
        }
    }

    //播放音效
    public void PlayEffectMusic(AudioClip audioClip)
    {
        if (playEffectMusic)
        {
            audioSource[1].PlayOneShot(audioClip);
        }
    }

    public void CloseBGMusic()
    {
        audioSource[0].Stop();
    }

    public void OpenBGMusic()
    {
        audioSource[0].Play();
    }

    public void CloseOrOpenBGMusic()
    {
        playBGMusic = !playBGMusic;
        if (playBGMusic)
        {
            OpenBGMusic();
        }
        else
        {
            CloseBGMusic();
        }
    }

    public void CloseOrOpenEffectMusic()
    {
        playEffectMusic = !playEffectMusic;
    }
}
