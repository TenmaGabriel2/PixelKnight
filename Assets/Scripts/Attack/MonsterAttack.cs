using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    public GameObject attack;

    public AttackCounter attackCounter;

    //角色伤害 
    public float damage;

    void Start()
    {
        attack = this.transform.Find("Attack").gameObject;
        attackCounter = attack.GetComponent<AttackCounter>();
    }
    public void Attack()
    {
        damage = MonsterDataManager.Instance.monsterData.atk;
        //Debug.Log("mon3er攻击"); //调试用
        StartCoroutine(AttackCoroutine(attack, attackCounter, damage ));
        SoundManager.Instance.PlaySound(4,"monsterAttack",false);

    }

    public virtual IEnumerator AttackCoroutine(GameObject attackCollider, AttackCounter attackCounter, float damage)
    {
        attackCounter.InitAttack(damage);
        attackCollider.SetActive(true);
        yield return new WaitForSeconds(0.3f);//0.3s检测时间
        attackCollider.SetActive(false);
    }
}
