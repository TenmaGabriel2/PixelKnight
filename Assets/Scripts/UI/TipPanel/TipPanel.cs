using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TipPanel : BasePanel
{
    public Button btnBack;
    public Button btnSure;
    public Text txtTip;
    public override void Init()
    {
        btnBack.onClick.AddListener(() =>
        {
            UIManager.Instance.HidePanel<TipPanel>();
        });
        btnSure.onClick.AddListener(() =>
        {
            //这里传入false，删除当前面板，不然切换场景后 面板销毁 但panelDic中的引用还是原来的
            //导致切换后无法显示 这里传入false直接删除 后面再创建
            PlayerDataManager.Instance.SavePlayerData();
            UIManager.Instance.HidePanel<TipPanel>(false);
            SceneManager.LoadScene("BeginScene");
        });
    }
    public void SetTip(string info)
    {
        txtTip.text = info;
    }
}
