using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectPanel : BasePanel
{
    public Button btnSure;
    public Button btnBack;
    public Button btnNext;
    public Button btnLast;
    public Text txtName;
    public Text txtLevel;
    public Image imgCharacter;

    private int nowIndex=0;
    public override void Init()
    {
        nowIndex = PlayerDataManager.Instance.allPlayerData.currentPlayerIndex; //上一次选择的角色

        RefreshCharacterInfo();

        btnSure.onClick.AddListener(() =>
        {
            //保存当前索引值 实现上次选择记忆
            PlayerDataManager.Instance.allPlayerData.currentPlayerIndex = nowIndex;
            PlayerDataManager.Instance.SavePlayerData();
            
            //隐藏自己 这里传false 删除面板 后面用再重新创 
            UIManager.Instance.HidePanel<CharacterSelectPanel>(false);
            //确定角色 跳转游戏场景
            SceneManager.LoadScene("GameScene");


        });
        btnBack.onClick.AddListener(() =>
        {
            //隐藏自己
            UIManager.Instance.HidePanel<CharacterSelectPanel>();
            UIManager.Instance.ShowPanel<BeginPanel>();
        });
        btnNext.onClick.AddListener(() =>
        {
            //切换角色索引到下一个
            NextCharacter();

        });
        btnLast.onClick.AddListener(() =>
        {
            LastCharacter();
        });
    }

    public void NextCharacter()
    {
        //切换角色索引到下一个
        if (nowIndex + 1 >= PlayerDataManager.Instance.allPlayerData.playerDataList.Count)
            return;
        if (nowIndex + 1 < PlayerDataManager.Instance.allPlayerData.playerDataList.Count)
            nowIndex++;

        RefreshCharacterInfo();

    }
    public void LastCharacter()
    {
        //切换角色索引到下一个
        if (nowIndex == 0)
            return;
        if (nowIndex >= 1)
            nowIndex--;

        RefreshCharacterInfo();
    }

    private void RefreshCharacterInfo()
    {
        //Debug.Log("当前index" + nowIndex);

        //得到当前索引的角色数据
        PlayerData playerData = PlayerDataManager.Instance.allPlayerData.playerDataList[nowIndex];
        //显示数据
        txtName.text = playerData.playerName;
        txtLevel.text = "Kill:" + playerData.monsterKillCount.ToString();
        imgCharacter.sprite = Resources.Load<Sprite>(playerData.playerIconPath);
    }
}
