using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData
{
    //标识
    public int playerID;
    public string playerName;
    public string playerIconPath;
    //基础属性
    public int level;
    public float exp;
    public float hp ;
    public float atk ;
    public float def ;
    public float speed;
    public float attackBackForce;

    public int monsterKillCount;
    public int tempKillCount;

    //背包
    //public List<ItemData> backpack = new List<ItemData>();
    //存可编辑物件
    public List<PlayerItem> backpack = new List<PlayerItem>();
     
}
