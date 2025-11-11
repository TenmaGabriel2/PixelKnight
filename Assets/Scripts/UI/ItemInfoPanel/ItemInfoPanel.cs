using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemInfoPanel : BasePanel
{
    public Button btnUse;
    public Button btnBack;
    public Text txtItemInfo;
    public Text txtItemCount;
    public Image imgItemIcon;

    private Item itemData;
    public override void Init()
    {

        btnUse.onClick.AddListener(() =>
        {
            //使用逻辑 物品数量减少 调用加成函数
            UseItem();
        });
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<ItemInfoPanel>();
        });
    }

    public void GetItemData(Item item)
    {
        itemData = item;
        if (itemData == null)
        {
            Debug.LogError("没得到数据");
            return;
        }

        //用ItemUI里的 item 数据更新面板 UI
        imgItemIcon.sprite = itemData.itemIcon; //显示物品图标
        txtItemInfo.text = itemData.itemInfo;   //显示物品描述
        //这个item是物品模版 显示数据还得用data里的数量
        //Debug.Log(item.itemName+item.itemID+"物品数量"+ PlayerDataManager.Instance.GetCurrentPlayerData().backpack[item.itemID].itemCount.ToString());
        txtItemCount.text = PlayerDataManager.Instance.GetCurrentPlayerData().backpack[item.itemID].itemCount.ToString();
    }

    public void UseItem()
    {
        //使用物品逻辑 物品数量减少 调用加成函数
        int id = itemData.itemID;
        if (PlayerDataManager.Instance.GetCurrentPlayerData().backpack[id].itemCount > 0)
        {
            //减少物品数量
            PlayerDataManager.Instance.GetCurrentPlayerData().backpack[id].itemCount--;
            //播放使用音效
            SoundManager.Instance.PlaySound(1,"playerUseItem",false);
            //调用加成函数
            PlayerDataManager.Instance.ApplyItemEffect(id);
            //更新UI
            txtItemCount.text = PlayerDataManager.Instance.GetCurrentPlayerData().backpack[id].itemCount.ToString();
            UIManager.Instance.GetPanel<BackpackPanel>().UpdateBackpack();
        }
        //物品数量为0 自动关闭面板
        if (PlayerDataManager.Instance.GetCurrentPlayerData().backpack[id].itemCount==0)
            UIManager.Instance.HidePanel<ItemInfoPanel>();
    }
}
