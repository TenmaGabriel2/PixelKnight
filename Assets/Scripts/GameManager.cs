using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //开始的时候根据选择的角色 创建角色对象
    void Start()
    {
        Instantiate(Resources.Load("Prefabs/Character/"+PlayerDataManager.Instance.GetCurrentPlayerData().playerID));
        UIManager.Instance.GetPanel<BackpackPanel>();
    }
}
