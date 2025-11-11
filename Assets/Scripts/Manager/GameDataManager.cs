using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataManager

{
    private static GameDataManager instance = new GameDataManager();
    public static GameDataManager Instance => instance;

    public MusicData musicData;

    private GameDataManager()
    {
        //初始化音乐数据
        musicData = JsonMgr.Instance.LoadData<MusicData>("MusicData");
    }

    public void SaveMusicData()
    {
        JsonMgr.Instance.SaveData(musicData, "MusicData");
    }
}
