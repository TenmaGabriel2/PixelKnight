using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemController : MonoBehaviour,IPointerClickHandler
{

    private ItemUI currentItem;

    public void OnPointerClick(PointerEventData eventData)
    {
        //点击到物品上，显示物品信息面板 将item信息传给面板
        ItemInfoPanel itemInfoPanel = UIManager.Instance.ShowPanel<ItemInfoPanel>();
        itemInfoPanel.GetItemData(currentItem.item);
    }

    void Start()
    {
        currentItem = GetComponent<ItemUI>();
    }
   
}
