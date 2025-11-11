using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterDataManager
{
    private static MonsterDataManager instance = new MonsterDataManager();
    public static MonsterDataManager Instance => instance;

    public MonsterData monsterData;

    private MonsterDataManager()
    {
        //读取JSON文件 
        monsterData = JsonMgr.Instance.LoadData<MonsterData>("MonsterData");
    }

    public void SaveMonsterData()
    {
        //保存为json文件
        JsonMgr.Instance.SaveData(monsterData, "MonsterData");
    }
}
