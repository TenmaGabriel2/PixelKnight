using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackpackPanel : BasePanel
{
    public Button btnBack;

    public Transform inventoryGrid;//背包格子
    public ItemUI itemPrefab; //物品预制体

    public override void Init()
    {
        btnBack.onClick.AddListener(() =>
        {
            //保存游戏数据 后面做成每隔1分钟自动保存一次 + 退出游戏的时候保存一次 11.5
            PlayerDataManager.Instance.SavePlayerData();

            UIManager.Instance.HidePanel<BackpackPanel>();
        });
        UpdateBackpack();
    }

    //更新背包内容
    public void UpdateBackpack()
    {
        //清空背包格子
        foreach (Transform child in inventoryGrid)
        {
            Destroy(child.gameObject);
        }
        PlayerData currentPlayer = PlayerDataManager.Instance.GetCurrentPlayerData();
        if (currentPlayer.backpack == null) return;

        foreach (var playerItem in currentPlayer.backpack)
        {
            if (PlayerDataManager.Instance.playerItemDict.TryGetValue(playerItem.itemID, out Item itemTemp))
            {
                if (currentPlayer.backpack[playerItem.itemID].itemCount >0) //如果背包中有物品且数量大于0就显示到背包
                {
                    ItemUI newItem = Instantiate(itemPrefab, inventoryGrid);
                    newItem.item = itemTemp;
                    newItem.icon.sprite = itemTemp.itemIcon;
                    newItem.itemCount.text = currentPlayer.backpack[playerItem.itemID].itemCount.ToString(); 
                }
            }
        }
    }
}
