using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player1 : Character
{
    //闪避相关
    [Header("闪避相关")]
    [SerializeField] private int maxDashCount = 2;
    private int currentDashCount = 0;
    [SerializeField] private float dashSpeed = 15;
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private float dashTime;

    //运动相关
    [Header("运动相关")]
    [SerializeField] private float runningSpeed = 4f;

    //跑步方向
    protected bool isMoving;
    //移动相关
    protected float horizontal;
    protected float vertical;
    protected Vector2 direction;

    [Header("受击相关")]
    [SerializeField] protected bool isInvincible;
    [SerializeField] protected float invincibleDuration = 2f;//无敌持续时间

    //转向
    protected float isTurn;

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        //闪避的冷却函数 后面可以封装成方法
        dashTime -= Time.deltaTime;
        if (dashTime <= -5)
        {
            dashTime = 0;
            currentDashCount = 0;
        }
        Movement();
        CheckInput();
    }
    /// <summary>
    /// 运动功能 包含转向、移动、闪避
    /// </summary>
    private void Movement()
    {
        if (isDead)
            return;
        //移动方向
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector2(horizontal, vertical).normalized;
        //竖直 vertical 水平 horizontal
        //animator的进入Run
        isMoving = Mathf.Abs(horizontal) + Mathf.Abs(vertical) > 0;
        animator.SetBool("isMoving", isMoving);
        //转向
        isTurn = Input.GetAxisRaw("Horizontal");
        if (isTurn < 0)
            spriteRenderer.flipX = true;
        if (isTurn > 0)
            spriteRenderer.flipX = false;

        //移动
        if (dashTime > 0) //闪避
        {
            rb.velocity = direction * dashSpeed;
        }
        else //移动
        {
            rb.velocity = direction * runningSpeed;
        }

        #region 限制移动时不能攻击 （不好玩）（可以考虑做成debuff
        ////移动
        //if (dashTime > 0) //闪避
        //{
        //    rb.velocity = direction * dashSpeed;
        //}
        //else if (isMoving && !isAttacking)
        //{
        //    rb.velocity = direction * runningSpeed;
        //}
        //else
        //{
        //    rb.velocity = Vector2.zero;
        //}
        #endregion

    }

    private void CheckInput()
    {
        if (isDead)
            return;

        //左键at1 右键at2
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
        {
            animator.SetBool("isAttacking", true);
            animator.SetTrigger("atk1");
        }
        if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K))
        {
            animator.SetBool("isAttacking", true);
            animator.SetTrigger("atk2");
        }

        #region 限制移动时不能攻击 （不好玩）
        //if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J)) && !isMoving)
        //{
        //    isAttacking = true;
        //    animator.SetBool("isAttacking", isAttacking);
        //    animator.SetTrigger("atk1");
        //}
        //if ((Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.K)) && !isMoving)
        //{
        //    isAttacking = true;
        //    animator.SetBool("isAttacking", isAttacking);
        //    animator.SetTrigger("atk2");
        //}
        #endregion

        ////这里是测试 受击动画 后面做受击触发
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    animator.SetTrigger("beAtk");
        //}
        ////这里是测试 死亡动画 后面做血量归零触发
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    animator.SetBool("isDead",true);
        //}
        //else
        //{
        //    animator.SetBool("isDead", false);
        //    //下面要做死亡的逻辑 即显示失败页面
        //}

        //shift闪避 (释放后开始计时 5s内最多用2次
        if (Input.GetKeyDown(KeyCode.LeftShift) && currentDashCount < maxDashCount)
        {
            dashTime = dashDuration;
            currentDashCount++;
        }
    }

    public override void Die()
    {
        base.Die();
        //播放玩家死亡音效

        //显示失败页面

    }

    public override void TakeDamage(float damage)
    {
        base.TakeDamage(damage);
        if (isInvincible)
            return;
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(InvincibleCoroutine());
        }
    }

    //无敌协程
    public virtual IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }
}
