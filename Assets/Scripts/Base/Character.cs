using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected Rigidbody2D rb;
    protected Animator animator;
    protected SpriteRenderer spriteRenderer;

    //攻击相关
    protected bool isAttacking;

    //血量相关
    [Header("属性")]
    [SerializeField]protected float maxHp;
    [SerializeField]protected float currentHp;

    protected bool isDead;

    protected Color originalColor;
    protected float fadeOutTime = 3f;

    //对象激活时满血
    protected virtual void OnEnabled()
    {
        currentHp = maxHp;
    }

    protected virtual void Start()
    {
       rb = GetComponent<Rigidbody2D>();
       spriteRenderer = GetComponent<SpriteRenderer>();
       animator = GetComponent<Animator>();
       originalColor = spriteRenderer.color;
    }

    protected virtual void Update()
    {
    }

    protected virtual void AttackOver()
    {
        isAttacking = false;
        animator.SetBool("isAttacking", isAttacking);
    }

    public virtual void TakeDamage(float damage)
    {
        if (isDead)
            return;
    }
    public virtual void Die()
    {
        if (isDead)
            return;

        isDead = true;
        //死亡  防止停止其他逻辑
        animator.SetBool("isDead", true);
        rb.velocity = Vector2.zero;

         StartCoroutine(FadeAndMoveToPool());

        //禁用脚本不方便销毁对象
        //enabled = false;
    }

  

    //死亡后销毁 但应该在场内存在3s左右
    protected virtual IEnumerator FadeAndMoveToPool()
    {
        yield return new WaitForSeconds(2f);//等放完死亡动画
        //过去的时间
        float pastTime = 0f;
        while (pastTime < fadeOutTime)
        {
            // 1~0线性插值计算透明度
            float alpha = Mathf.Lerp(1f, 0f, pastTime / fadeOutTime);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            pastTime += Time.deltaTime;
            yield return null;
        }

        //后面写MoveToPool逻辑 Unity自带的对象池 
        //怪物、boss类中实现 玩家只需要Fade后面再显示失败页面什么的 怪物要ToPool 
    }
}
