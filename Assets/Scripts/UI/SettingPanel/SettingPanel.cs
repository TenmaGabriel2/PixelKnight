using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : BasePanel
{
    public Button btnBack;
    public Toggle toggleMusic;
    public Toggle toggleSound;
    public Slider sliderMusic;
    public Slider sliderSound;
    public override void Init()
    {
        //初始化UI内容
        MusicData data = GameDataManager.Instance.musicData;
        toggleMusic.isOn = data.isMusicOpen;
        toggleSound.isOn = data.isSoundOpen;
        sliderMusic.value = data.musicVolume;
        sliderSound.value = data.soundVolume;

        //绑定事件
        btnBack.onClick.AddListener(() =>
        {
            //保存数据
            GameDataManager.Instance.SaveMusicData();
            //隐藏自己
            UIManager.Instance.HidePanel<SettingPanel>();
        });

        toggleMusic.onValueChanged.AddListener((b) =>
        {
            BKMusic.Instance.OpenMusic(b);
            GameDataManager.Instance.musicData.isMusicOpen = b;
        });

        toggleSound.onValueChanged.AddListener((b) =>
        {
            //操控音效开关
            SoundManager.Instance.OpenSound(b);
            //记录音效数据
            GameDataManager.Instance.musicData.isSoundOpen = b;
        });

        sliderMusic.onValueChanged.AddListener((v) =>
        {
            BKMusic.Instance.ChangeMusicVolume(v);
            GameDataManager.Instance.musicData.musicVolume = v;
        });

        sliderSound.onValueChanged.AddListener((v) =>
        {
            SoundManager.Instance.ChangeSoundVolume(v);
            GameDataManager.Instance.musicData.soundVolume = v;
        });
    }
}
