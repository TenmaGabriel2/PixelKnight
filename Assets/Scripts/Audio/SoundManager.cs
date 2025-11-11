using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    public static SoundManager Instance => instance;


    public AudioClip playerLightAttack;
    public AudioClip playerHeavyAttack;
    public AudioClip playerTakeDamage;
    public AudioClip playerDeath;
    public AudioClip playerDash;
    public AudioClip skillCoolDown;

    public AudioClip monsterAttack;
    public AudioClip monsterDeath;

    public AudioClip playerUseItem;
    public AudioClip gameOver;


    public List<AudioSource> soundSources; //多通道防止音效重叠
    public int audioCount=6;
    private void Awake()
    {
        instance = this;
        MusicData data = GameDataManager.Instance.musicData;

        //遍历创建 并初始化音量
        for ( int i= 0; i < audioCount; i++)
        {
            AudioSource soundSource = gameObject.AddComponent<AudioSource>();
            soundSource.volume = data.soundVolume;
            soundSource.mute = !data.isSoundOpen;
            soundSources.Add(soundSource);
        }


    }
    /// <summary>
    /// 播放音效的方法
    /// </summary>
    /// <param name="index">指定audio通道</param>
    /// <param name="name">播放的音效名字</param>
    /// <param name="isLoop">是否循环</param>
    public void PlaySound(int index,string name,bool isLoop)
    {
        AudioClip soundClip = GetSoundClip(name);
        if(soundClip!= null)
        {
            AudioSource soundSource = soundSources[index];
            soundSource.clip = soundClip;
            soundSource.loop = isLoop;
            soundSource.Play();
        }
    }

    //通过名字获取音效
    public AudioClip GetSoundClip(string name)
    {
        switch (name)
        {
            case "playerLightAttack":
                return playerLightAttack;
            case "playerHeavyAttack":                
                return playerHeavyAttack;
            case "playerTakeDamage":
                return playerTakeDamage;
            case "playerDeath":
                return playerDeath;
            case "monsterAttack":
                return monsterAttack;
            case "monsterDeath":
                return monsterDeath;
            case "playerDash":
                return playerDash;
            case "skillCoolDown":
                return skillCoolDown;
            case "playerUseItem":
                return playerUseItem;
            case "gameOver":
                return gameOver;
        }
        return null;
    }

    public void OpenSound(bool isOpen)
    {
       foreach(var sound in soundSources)
        {
            sound.mute = !isOpen;
        }
    }
    public void ChangeSoundVolume(float volume)
    {
        foreach(var sound in soundSources)
        {
            sound.volume = volume;
        }
    }
}
