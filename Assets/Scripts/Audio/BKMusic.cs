using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BKMusic : MonoBehaviour
{
    private static BKMusic instance;
    public static BKMusic Instance => instance;

    private AudioSource bkMusicSource;
    private void Awake()
    {
        instance = this;
        bkMusicSource = GetComponent<AudioSource>();

        //通过数据得到音乐大小
        MusicData data = GameDataManager.Instance.musicData;
        OpenMusic(data.isMusicOpen);
        ChangeMusicVolume(data.musicVolume);
    }

    public void OpenMusic(bool isOpen)
    {
        bkMusicSource.mute = !isOpen;
    }
    public void ChangeMusicVolume(float volume)
    {
        bkMusicSource.volume = volume;
    }
}
