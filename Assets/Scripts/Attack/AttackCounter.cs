using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class AttackCounter : MonoBehaviour
{
    private float currentDamage; // 当前攻击的伤害值

    //角色调用 传入伤害
    public void InitAttack(float damage)
    {
        currentDamage = damage;
    }

    //当碰撞器检测到其他对象进入时
    private void OnTriggerEnter2D(Collider2D other)
    {
        //这里处理Player 后面可以做击中其他物体消失的效果
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            Vector2 directionPlayer = (player.transform.position - this.transform.position).normalized;
            // 调用玩家的TakeDamage
            player.TakeDamage(currentDamage);
            // 击中后清空玩家速度 
            player.rb.velocity = Vector2.zero;
            player.rb.AddForce(directionPlayer * MonsterDataManager.Instance.monsterData.attackBackForce, ForceMode2D.Impulse);
        }
        //这里处理Monster 后面可以做击中其他物体消失的效果
        if (other.CompareTag("Monster"))
        {
            //得到自己 后面判断 monster攻击monster不掉血
            GameObject attacker = this.transform.parent.gameObject;
            if (attacker.CompareTag("Monster")&&other.CompareTag("Monster"))
                return;

            //得到敌人
            Monster monster = other.GetComponent<Monster>();
            //得到计算方向向量（玩家到怪物的方向）
            Vector2 directionMonster = (monster.transform.position - this.transform.position).normalized;
            //调用怪物的TakeDamage
            monster.TakeDamage(currentDamage);
            //击中后清空怪物速度并击退一下
            monster.rb.velocity = Vector2.zero;
            monster.rb.AddForce(directionMonster* PlayerDataManager.Instance.GetCurrentPlayerData().attackBackForce, ForceMode2D.Impulse);
        }
    }
}
