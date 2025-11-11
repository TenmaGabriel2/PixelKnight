using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject lightAttack;
    public GameObject heavyAttack;

    public AttackCounter lightAttackCounter;
    public AttackCounter heavyAttackCounter;

    //角色伤害 
    public float damage;

    void Start()
    {
        lightAttack = this.transform.Find("LightAttack").gameObject;
        lightAttackCounter = lightAttack.GetComponent<AttackCounter>();

        heavyAttack = this.transform.Find("HeavyAttack").gameObject;
        heavyAttackCounter = heavyAttack.GetComponent<AttackCounter>();
    }

    //轻击--在动画中调用 
    public void LightAttack()
    {
        damage = PlayerDataManager.Instance.GetCurrentPlayerData().atk;
        StartCoroutine(AttackCoroutine(lightAttack, lightAttackCounter, damage));
        SoundManager.Instance.PlaySound(2, "playerLightAttack", false);
    }

    //重击检测
    public void HeavyAttack()
    {
        damage = PlayerDataManager.Instance.GetCurrentPlayerData().atk;
        StartCoroutine(AttackCoroutine(heavyAttack, heavyAttackCounter, damage * 2));
        SoundManager.Instance.PlaySound(2, "playerHeavyAttack", false);
    }

    public virtual IEnumerator AttackCoroutine(GameObject attackCollider, AttackCounter attackCounter, float damage)
    {
        attackCounter.InitAttack(damage);
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(0.3f);//0.3s检测时间
        attackCollider.SetActive(false);
    }
}
