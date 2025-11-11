using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillDataManager
{
    private static SkillDataManager instance = new SkillDataManager();
    public static SkillDataManager Instance => instance;


    public SkillData skill;

    private SkillDataManager()
    {
        skill = JsonMgr.Instance.LoadData<SkillData>("PlayerSkill");

    }
    public void SavePlayerSkill()
    {
        //保存为json文件
        JsonMgr.Instance.SaveData(skill, "PlayerSkill");
    }


}
