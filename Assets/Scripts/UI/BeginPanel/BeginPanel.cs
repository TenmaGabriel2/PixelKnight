using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class BeginPanel : BasePanel
{
    public Button btnStart;
    public Button btnSetting;
    public Button btnQuit;
    public override void Init()
    {
        btnStart.onClick.AddListener(() =>
        {
            //切换到角色选择
            UIManager.Instance.ShowPanel<CharacterSelectPanel>();
            UIManager.Instance.HidePanel<BeginPanel>();
        });
        btnSetting.onClick.AddListener(() =>
        {
            //打开设置面板
            UIManager.Instance.ShowPanel<SettingPanel>();

        });
        btnQuit.onClick.AddListener(() =>
        {
            //退出游戏
            Application.Quit();
        });
    }

}
