using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager
{
    private static UIManager instance = new UIManager();
    public static UIManager Instance => instance;

    //存储面板的容器
    private Dictionary<string, BasePanel> panelDic = new Dictionary<string, BasePanel>();
    //得到canvas对象 方便设置其子对象
    private Transform canvasTrans;
    private UIManager()
    {
        // 初始场景的Canvas引用（场景切换后会被覆盖）
        UpdateCanvasReference();
        // 监听场景加载事件
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 场景加载完成后更新Canvas引用
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateCanvasReference();
    }

    // 刷新Canvas引用为当前场景的Canvas
    private void UpdateCanvasReference()
    {
        //Canvas currentCanvas = GameObject.FindObjectOfType<Canvas>(); 直接找跟血条ui冲突 可能显示错乱
        GameObject currentCanvas = GameObject.FindWithTag("UICanvas");
        if (currentCanvas != null)
        {
            canvasTrans = currentCanvas.transform;
        }
        else
        {
            Debug.LogError("没canvas");
        }
    }
    //显示面板
    public T ShowPanel<T>() where T : BasePanel
    {
        //需保证T的类型与面板名一致 方便使用
        string panelName = typeof(T).Name;
        //Debug.Log("显示面板" + panelName);

        //判断是否已经有这个面板 如果有直接返回给外部使用
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;
        //显示面板 即动态创建预设体 并设置其父对象
        GameObject panelObj = GameObject.Instantiate(Resources.Load<GameObject>("UI/" + panelName));
        panelObj.transform.SetParent(canvasTrans, false);

        //接着 得到对应面板脚本 并存储 方便后续获取
        T panel = panelObj.GetComponent<T>();
        panelDic.Add(panelName, panel);
        //调用显示自己的逻辑
        panel.ShowMe();

        return panel;
    }

    /// <summary>
    /// 隐藏面板的方法
    /// </summary>
    /// <typeparam name="T">panel类</typeparam>
    /// <param name="isFade">隐藏 传入false则删除面板</param>
    public void HidePanel<T>(bool isFade = true) where T : BasePanel
    {
        //同理 
        string panelName = typeof(T).Name;
        //判断当前显示的面板有无这个名字的面板
        if (panelDic.ContainsKey(panelName))
        {
            if (isFade)
            {
                panelDic[panelName].HideMe(() =>
                {
                    //面板淡出成功后 希望删除面板
                    GameObject.Destroy(panelDic[panelName].gameObject);
                    //删除面板后 从字典中移除
                    panelDic.Remove(panelName);
                });
            }
            else
            {
                //删除面板
                GameObject.Destroy(panelDic[panelName].gameObject);
                //从字典中移除
                panelDic.Remove(panelName);
            }
        }
    }
    //获得面板
    public T GetPanel<T>() where T : BasePanel
    {
        string panelName = typeof(T).Name;
        //有就返回
        if (panelDic.ContainsKey(panelName))
            return panelDic[panelName] as T;

        //没有就返回空
        return null;

    }
}
