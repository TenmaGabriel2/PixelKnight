using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerState;

public class Player : MonoBehaviour
{
    [Header("数值")]
    public float currentHp;
    public bool isAttackOver;
    public bool isHurtOver;
    public bool isDead;
    protected Color originalColor;
    protected float fadeOutTime = 3f;

    [Header("受击相关")]
    [SerializeField] protected bool isInvincible;
    [SerializeField] protected float invincibleDuration = 2f;//无敌持续时间

    #region 组件
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }

    #endregion

    #region 状态
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }

    public PlayerLightAttack lightAttack { get; private set; }

    public PlayerHeavyAttack heavyAttack { get; private set; }

    public PlayerHurt hurtState { get; private set; }

    #endregion

    public PlayerHealthBar playerHealthBar;

    private void Awake()
    {
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this,stateMachine,"isIdle", E_AnimatorParamType.Bool);
        moveState = new PlayerMoveState(this,stateMachine,"isMoving", E_AnimatorParamType.Bool);
        lightAttack = new PlayerLightAttack(this,stateMachine,"lightAttack", E_AnimatorParamType.Trigger);
        heavyAttack = new PlayerHeavyAttack(this,stateMachine,"heavyAttack", E_AnimatorParamType.Trigger);
        hurtState = new PlayerHurt(this,stateMachine,"beHurt", E_AnimatorParamType.Trigger);
    }

    private void Start()
    {
        animator = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody2D>();
        sr = this.GetComponent<SpriteRenderer>();

        stateMachine.Initialize(idleState);

        currentHp = PlayerDataManager.Instance.GetCurrentPlayerData().hp;
        originalColor = sr.color;
        playerHealthBar = GetComponent<PlayerHealthBar>();
    }

    private void Update()
    {
        stateMachine.currentState.Update();
        SkillDataManager.Instance.skill.dashTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkillDataManager.Instance.skill.Dash(this);
        }

        //按住alt显示UI面板
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            UIManager.Instance.ShowPanel<PlayerPanel>();
        }
        if (Input.GetKeyUp(KeyCode.LeftAlt))
        {
            UIManager.Instance.HidePanel<PlayerPanel>();
        }
        Win();
    }

    
    public IEnumerator DashCoroutine()
    {
        yield return new WaitForSeconds(3f);
        SkillDataManager.Instance.skill.currentDashCount--;
    }

    public void SetVelocity(float xVelocity, float yVelocity)
    {
        rb.velocity = new Vector2(xVelocity, yVelocity);
    }
    //面朝向相关 （用filp不能翻转碰撞盒
    public void SetFilp(float xInput)
    {
        // 保持当前比例
        Vector3 currentScale = transform.localScale;
        float originalScaleX = Mathf.Abs(currentScale.x);
        if (xInput > 0)
            transform.localScale = new Vector3(originalScaleX, transform.localScale.y, transform.localScale.z);
        //sr.flipX = false;
        else if (xInput < 0)
            transform.localScale = new Vector3(-originalScaleX, transform.localScale.y, transform.localScale.z);
        //sr.flipX = true;

    }
    
    
    public void AttackOver()
    {
        isAttackOver = true;
    }

    public void HurtOver()
    {
        isHurtOver = true;
    }

  

    public void TakeDamage(float damage)
    {
        if (isDead)
            return;
        if (isInvincible)
            return;
        currentHp -= damage;
        stateMachine.ChangeState(hurtState);
        playerHealthBar.UpdateHPBar();
        if (currentHp <= 0)
        {
            Die();
        }
        else
        {
            SoundManager.Instance.PlaySound(1, "playerTakeDamage", false);
            isHurtOver = true;
            StartCoroutine(InvincibleCoroutine());
        }
    }

    public void Die()
    {
        if (isDead)
            return;
        isDead = true;
        //死亡  防止停止其他逻辑
        SoundManager.Instance.PlaySound(1, "playerDeath", false);
        animator.SetBool("isDead", true);
        rb.velocity = Vector2.zero;

        StartCoroutine(FadeOut());
    }

    //无敌协程
    public virtual IEnumerator InvincibleCoroutine()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibleDuration);
        isInvincible = false;
    }
    protected virtual IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(2f);//等放完死亡动画
        //过去的时间
        float pastTime = 0f;
        while (pastTime < fadeOutTime)
        {
            // 1~0线性插值计算透明度
            float alpha = Mathf.Lerp(1f, 0f, pastTime / fadeOutTime);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            pastTime += Time.deltaTime;
            yield return null;
        }
        //这里应该显示游戏结束画面
    }

    public void Win()
    {
        if (PlayerDataManager.Instance.GetCurrentPlayerData().tempKillCount >=20) {
            PlayerDataManager.Instance.GetCurrentPlayerData().tempKillCount = 0;
            //显示提示面板
            SoundManager.Instance.PlaySound(5, "gameOver", false);
            UIManager.Instance.ShowPanel<TipPanel>();
            UIManager.Instance.ShowPanel<TipPanel>().SetTip("恭喜你取得阶段性胜利！\n 是否返回初始界面？");
        }
    }
}
