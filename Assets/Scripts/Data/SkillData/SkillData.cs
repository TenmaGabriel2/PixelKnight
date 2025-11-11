using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillData
{
    public int dashCount = 2;
    public int currentDashCount = 0;
    public float dashDuration = 0.1f;
    public float dashTime = 0;
    public bool isDash;
    public float dashSpeed = 25;



    public void Dash( Player player )
    {
        if (currentDashCount >= dashCount) { 
            SoundManager.Instance.PlaySound(0, "skillCoolDown", false);
            return;//这里播放冷却语音
        }
        //移动
        dashTime = 0;
        currentDashCount++;
        SoundManager.Instance.PlaySound(0, "playerDash", false);
        player.StartCoroutine(player.DashCoroutine());
    }
}
