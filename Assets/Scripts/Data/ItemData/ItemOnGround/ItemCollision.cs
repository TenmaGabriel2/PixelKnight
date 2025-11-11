using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ItemCollision : MonoBehaviour
{

    public Item item;
    public SpriteRenderer sr;
    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = item.itemIcon;
    }
    //碰撞后添加道具到玩家背包
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AddItemToPlayer();
            Destroy(gameObject);
        }
    }

    public void AddItemToPlayer()
    {
        PlayerData currentPlayer = PlayerDataManager.Instance.GetCurrentPlayerData();
        bool isStacked = false;
        foreach (var playerItem in currentPlayer.backpack)
        {
            if (playerItem.itemID == item.itemID)
            {
                playerItem.itemCount += 1;
                isStacked = true;
                break;
            }
        }

        // 未堆叠则新增 PlayerItem
        if (!isStacked)
        {
            currentPlayer.backpack.Add(new PlayerItem
            {
                itemID = item.itemID,
                itemCount = 1
            });
        }
    }
}
