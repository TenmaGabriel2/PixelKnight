using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataManager
{
    private static PlayerDataManager instance = new PlayerDataManager();
    public static PlayerDataManager Instance => instance;

    public AllPlayerData allPlayerData;

    public Player currentPlayer;


    //存物品信息的字典
    public Dictionary<int, Item> playerItemDict = new Dictionary<int, Item>();

    //构造函数读取JSON文件 后面对数据的操作要对这里进行
    private PlayerDataManager()
    {
        LoadItemDict();
        //读取JSON文件 
        allPlayerData = JsonMgr.Instance.LoadData<AllPlayerData>("AllPlayerData");
        // 首次加载时初始化角色数据
        if (allPlayerData.playerDataList.Count == 0)
        {
            Debug.Log("初始化角色数据？");
            allPlayerData.playerDataList.Add(new PlayerData
            {
                playerID = 0,
                playerName = "金毛骑士",
                playerIconPath = "CharacterSprite/Knight",
                level = 1,
                hp = 150,
                atk = 15,
                def = 10,
                speed = 4,
                attackBackForce = 4,
                backpack = new List<PlayerItem>()//背包
            });

            allPlayerData.playerDataList.Add(new PlayerData
            {
                playerID = 1,
                playerName = "蓝毛法师",
                playerIconPath = "CharacterSprite/Wizard",
                level = 1,
                hp = 100,
                atk = 25,
                def = 5,
                speed = 4,
                attackBackForce = 4,
                backpack = new List<PlayerItem>()
            });

            SavePlayerData();    
        } 
    }

    public void SavePlayerData()
    {
        //保存为json文件
        JsonMgr.Instance.SaveData(allPlayerData, "AllPlayerData");
    }

    //用于获取当前角色数据
    public PlayerData GetCurrentPlayerData()
    {
        return allPlayerData.playerDataList[allPlayerData.currentPlayerIndex];
    }

    //获取当前物品信息到字典中 方便后面使用
    public void LoadItemDict()
    {
        //加载所有Item
        Item[] allItems = Resources.LoadAll<Item>("Prefabs/Inventory/Items");
        if (allItems.Length == 0)
        {
            Debug.LogError("路径出错？？？");
            return;
        }
        //itemID唯一
        foreach (var item in allItems)
        {
            if (!playerItemDict.ContainsKey(item.itemID))
            {
                playerItemDict.Add(item.itemID, item);
            }
        }
    }

    public void GetPlayer()
    {
        currentPlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    //使用背包物品的效果
    public void ApplyItemEffect(int itemID)
    {
        //获取当前角色player 方便进行数值操作
        GetPlayer();

        //攻击力什么的直接调用的player中的数值 这里直接改就好
        //角色的currentHp在player初始化中得到 这里需要得到player再改
        //血条在player中有 与血量有关的需要更新血条
        switch (itemID)
        {
            case 0:
                //恶魔契约+50atk
                GetCurrentPlayerData().atk += 50;
                break;
            case 1:
                //下等力量+1atk
                GetCurrentPlayerData().atk += 1;
                break;
            case 2:
                //神圣祝福 +50hp （生命上限
                GetCurrentPlayerData().hp += 50;
                currentPlayer.currentHp += 50;
                currentPlayer.playerHealthBar.UpdateHPBar();
                break;
            case 3:
                //下等生命 +10hp 当前生命值
                //得到当前角色信息
                if (currentPlayer.currentHp + 10 <= GetCurrentPlayerData().hp)
                currentPlayer.currentHp += 10;
                else
                currentPlayer.currentHp = GetCurrentPlayerData().hp;

                currentPlayer.playerHealthBar.UpdateHPBar();
                break;

            //后面根据id接着加即可
        }
    }

    //重载一手 用物品名字调用
    public void ApplyItemEffect(string itemName)
    {
        //获取当前角色player 方便进行数值操作
        GetPlayer();

        //攻击力什么的直接调用的player中的数值 这里直接改就好
        //角色的currentHp在player初始化中得到 这里需要得到player再改
        //血条在player中有 与血量有关的需要更新血条
        switch (itemName)
        {
            case "神秘恶魔契约":
                //恶魔契约+50atk
                GetCurrentPlayerData().atk += 50;
                break;
            case "下等力量秘药":
                //下等力量+1atk
                GetCurrentPlayerData().atk += 1;
                break;
            case "神圣祝福药水":
                //神圣祝福 +50hp （生命上限
                GetCurrentPlayerData().hp += 50;
                currentPlayer.currentHp += 50;
                currentPlayer.playerHealthBar.UpdateHPBar();
                break;
            case "生命药剂":
                //下等生命 +10hp 当前生命值
                //得到当前角色信息
                if (currentPlayer.currentHp + 10 <= GetCurrentPlayerData().hp)
                    currentPlayer.currentHp += 10;
                else
                    currentPlayer.currentHp = GetCurrentPlayerData().hp;

                currentPlayer.playerHealthBar.UpdateHPBar();
                break;

                //后面根据名字接着加即可
        }
    }
}
