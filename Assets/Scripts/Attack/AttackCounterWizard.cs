using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCounterWizard : MonoBehaviour
{
    private float currentDamage; // 当前攻击的伤害值
    float destroyTimer = 2;
    float time = 0;
    //角色调用 传入伤害
    public void InitAttack(float damage)
    {
        currentDamage = damage;
    }
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= destroyTimer)
        {
            time = 0;
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Monster"))
        { 
            //得到敌人
            Monster monster = other.GetComponent<Monster>();
            //得到计算方向向量（玩家到怪物的方向）
            Vector2 directionMonster = (monster.transform.position - this.transform.position).normalized;
            //调用怪物的TakeDamage
            monster.TakeDamage(currentDamage);
            //击中后清空怪物速度并击退一下
            monster.rb.velocity = Vector2.zero;
            monster.rb.AddForce(directionMonster * PlayerDataManager.Instance.GetCurrentPlayerData().attackBackForce, ForceMode2D.Impulse);
            Destroy(this.gameObject);
        }else if (other.CompareTag("Player"))
        {
            //防止碰到自己的碰撞盒后销毁
        }
        else
        {
            Destroy(this.gameObject);
        } 
    }
}
