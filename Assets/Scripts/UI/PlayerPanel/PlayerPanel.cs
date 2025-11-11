using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel : BasePanel
{
    public Button btnBackPack;
    public Button btnSetting;
    public Button btnBack;

    public Text atkCount;
    public Text maxHpCount;
    public Text currentHpCount;
    public Text currentKillCount;

    public override void Init()
    {
        UpdatePlayerPanel();

        btnBackPack.onClick.AddListener(() =>
        {
            //后面写了背包面板再启用
            UIManager.Instance.ShowPanel<BackpackPanel>();
        });
        btnSetting.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<SettingPanel>();
        });
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.ShowPanel<TipPanel>();
            UIManager.Instance.ShowPanel<TipPanel>().SetTip("是否返回初始界面？");
        });
    }
    
    public void UpdatePlayerPanel()
    {
        PlayerDataManager.Instance.GetPlayer();
        atkCount.text = "攻击力\n <size=50>" + PlayerDataManager.Instance.GetCurrentPlayerData().atk.ToString() + "</size>";
        maxHpCount.text = "最大生命值\n <size=50>" + PlayerDataManager.Instance.GetCurrentPlayerData().hp.ToString() + "</size>";
        currentKillCount.text = "击杀数量\n <size=50>" + PlayerDataManager.Instance.GetCurrentPlayerData().monsterKillCount.ToString() + "</size>";
        currentHpCount.text = "当前生命值\n <size=50>" + PlayerDataManager.Instance.currentPlayer.currentHp.ToString() + "</size>";
        
    }
}
