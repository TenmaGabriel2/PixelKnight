using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Image hpBK;//底图
    public Image hpImg;//血条
    public Image hpEffectImg;//缓动血条

    public float maxHP;
    public float currentHP;
    public float hpChangeTime = 0.5f;

    //血条消失相关
    private Color originalColor;
    private float fadeOutTime = 4f;

    //用于得到实时血量
    private Player player;

    private Coroutine hpChangeCoroutine;

    private void Start()
    {
        InitHPBar();
    }

    //初始化血条
    public void InitHPBar()
    {
        maxHP = PlayerDataManager.Instance.GetCurrentPlayerData().hp;
        currentHP = maxHP;
        player = GetComponent<Player>();

        //初始更新血条
        hpImg.fillAmount = currentHP / maxHP;
        hpEffectImg.fillAmount = currentHP / maxHP;
        //记录初始颜色
        originalColor = hpBK.color;
    }
    //更新血条的方法 在takeDamage中调用
    public void UpdateHPBar()
    {
        maxHP = PlayerDataManager.Instance.GetCurrentPlayerData().hp;
        currentHP = player.currentHp;
        hpImg.fillAmount = currentHP / maxHP;
        if (hpChangeCoroutine != null)
        {
            StopCoroutine(hpChangeCoroutine);
        }
        hpChangeCoroutine = StartCoroutine(ChangeHP());
        if (currentHP <= 0)
        {
            StartCoroutine(HPBarFadeOut());
        }

    }
    /// <summary>
    /// 缓动血条的协程
    /// </summary>
    /// <returns></returns>
    private IEnumerator ChangeHP()
    {
        float hpLength = hpEffectImg.fillAmount - hpImg.fillAmount;
        float usedTime = 0;
        while (usedTime < hpChangeTime && hpLength != 0)
        {
            usedTime += Time.deltaTime;
            hpEffectImg.fillAmount = Mathf.Lerp(hpImg.fillAmount + hpLength, hpImg.fillAmount, usedTime / hpChangeTime);
            yield return null;
        }
        hpEffectImg.fillAmount = hpImg.fillAmount;
    }

    //血条消失的协程
    private IEnumerator HPBarFadeOut()
    {
        yield return new WaitForSeconds(1f);//等放完动画

        float pastTime = 0f;
        while (pastTime < fadeOutTime)
        {
            // 计算透明度
            float alpha = Mathf.Lerp(1f, 0f, pastTime / fadeOutTime);
            pastTime += Time.deltaTime;
            // 设置透明度 调用这个时候 其他的血条已经消失了 直接改底色得了
            hpBK.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            pastTime += Time.deltaTime;
            yield return null;
        }
    }

    //血条透明度恢复方法
    public void RecoverHPBar()
    {
        hpBK.color = originalColor;
    }
}
