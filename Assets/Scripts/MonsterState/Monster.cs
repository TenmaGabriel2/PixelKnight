using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using Pathfinding;
using static MonsterState;

public class Monster: MonoBehaviour
{
    [Header("数值")]
    public float currentHp;
    public bool isAttackOver;
    public bool isHurtOver = true;
    public bool isDead;
    protected Color originalColor;
    public Rigidbody2D originalRb;
    protected float fadeOutTime = 3f;


    #region 组件
    public Animator animator { get; private set; }
    public Rigidbody2D rb { get; private set; }
    public SpriteRenderer sr { get; private set; }

    #endregion

    #region 状态
    public MonsterStateMachine stateMachine { get; private set; }
    public MonsterIdleState idleState { get; private set; }
    public MonsterMoveState moveState { get; private set; }
    public MonsterAttackState attackState { get; private set; }
    public MonsterHurtState hurtState { get; private set; }
    #endregion

    #region 寻路相关
    public Seeker seeker;
    public List<Vector3> pathPointList;
    public int currentPathPointIndex;

    public Transform target;//目标
    public Vector2 targetDirection;//目标方向


    #endregion

    //巡逻相关 用巡逻移动时间来控制
    public float patrolTime = 3f; 
    public float patrolSpeed = 2f; 
    public bool isPatroling;
    public Coroutine patrolCoroutine;

    //得到血条
    private HealthBar healthBar;

    private Loot lootPool;
    private void Awake()
    {
        stateMachine = new MonsterStateMachine();
        idleState = new MonsterIdleState(this, stateMachine,"isIdle", E_AnimatorParamType.Bool);
        moveState = new MonsterMoveState(this, stateMachine,"isMoving", E_AnimatorParamType.Bool);
        attackState = new MonsterAttackState(this, stateMachine,"lightAttack", E_AnimatorParamType.Trigger);
        hurtState = new MonsterHurtState(this, stateMachine,"isHurt", E_AnimatorParamType.Trigger);

        seeker = GetComponent<Seeker>();
        lootPool = Resources.Load<Loot>("Prefabs/Potion/LootPool");

    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        stateMachine.Initialize(idleState);

        currentHp = MonsterDataManager.Instance.monsterData.hp;
        originalColor = sr.color;
        originalRb = rb;

        StartCoroutine(GetTarget());

        healthBar = GetComponent<HealthBar>();
    }
    private void Update()
    {
        stateMachine.currentState.Update();
        //追击（包含面朝向更新
        if (isHurtOver)
        {
            //Debug.Log("正在调用追击" + isHurtOver);
            PursueTarget();
        }

    }
    //两个动画事件
    public void AttackOver()
    {
        //Debug.Log("调用我了？？？"); 
        isAttackOver = true;
    }

    public void HurtOver()
    {
        isHurtOver = true;
        animator.SetBool("isHurtOver", isHurtOver);

    }

    //受伤
    public void TakeDamage(float damage)
    {
        if (isDead)
            return;
        isHurtOver = false;
        animator.SetBool("isHurtOver",false);
        stateMachine.ChangeState(hurtState);
        currentHp -= damage;
        healthBar.UpdateHPBar();
        if (currentHp <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead)
            return;
        isDead = true;
        //死亡  防止停止其他逻辑
        SoundManager.Instance.PlaySound(3, "monsterDeath", false);
        animator.SetBool("isDead", true);
        rb.velocity = Vector2.zero;
        rb.mass = 1000; //设个大质量 尸体踢不动 存池子要改
        //击杀数++
        PlayerDataManager.Instance.GetCurrentPlayerData().monsterKillCount++;
        PlayerDataManager.Instance.GetCurrentPlayerData().tempKillCount++;
        //爆金币
        DropLoot();
        StartCoroutine(FadeOutAndMoveToPool());
    }

    protected virtual IEnumerator FadeOutAndMoveToPool()
    {
        yield return new WaitForSeconds(0.5f);//等放完死亡动画
        //过去的时间
        float pastTime = 0f;
        while (pastTime < fadeOutTime)
        {
            // 计算透明度
            float alpha = Mathf.Lerp(1f, 0f, pastTime / fadeOutTime);
            sr.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);

            pastTime += Time.deltaTime;
            yield return null;
        }
        //这里应该爆金币 然后MoveToPool
        if (MonsterPoolManager.Instance != null)
        {
            ReSetState();
            MonsterPoolManager.Instance.RecycleMonster(this);
        }
    }

    //回收到池子前重置下状态
    private void ReSetState()
    {
        //重置重量、颜色、血条
        rb.mass = 1f;
        sr.color = originalColor;
        currentHp = MonsterDataManager.Instance.monsterData.hp;
        isDead = false;
        isAttackOver = true;
        isHurtOver = true;
        isPatroling = false;
        stateMachine.ChangeState(moveState);//切换一个状态 再次激活时会进入idle
        healthBar.RecoverHPBar();
        healthBar.InitHPBar();
    }

    #region 寻路相关
    //寻路
    public void GetPath(Vector3 target)
    {
        currentPathPointIndex = 0;
        seeker.StartPath(transform.position, target, path =>
        {
            pathPointList = path.vectorPath;
        });
    }

    public void AutoUpdatePath()
    {

        //没东西就刷一次
        if (pathPointList == null || pathPointList.Count <= 0)
        {
            GetPath(target.position);
        }
        else if (Vector2.Distance(transform.position, pathPointList[currentPathPointIndex]) <= 0.5f)
        {
            //追到就下一个点
            currentPathPointIndex++;
            if (currentPathPointIndex >= pathPointList.Count)
                GetPath(target.position);
        }
    }
    //追击目标
    public void PursueTarget()
    {
        if (target == null)
            return;

        if (isDead)
            return;

        float distance = Vector2.Distance(transform.position, target.position) - 0.5f;
        if (distance <= MonsterDataManager.Instance.monsterData.pursueDistance )
        {
            isPatroling = false;
            StopCoroutine(Patrol());

            //加载路径
            AutoUpdatePath();
            //更新面朝向
            AutoUpdateFacing();

            targetDirection = (target.position - transform.position).normalized;

            if (distance <= MonsterDataManager.Instance.monsterData.attackDistance)
            {
                stateMachine.ChangeState(attackState);//攻击状态
                //伤害检测加到动画事件里
            }
            else
            {
                stateMachine.ChangeState(moveState);//移动状态 追逐
            }
        }
        else
        {
            isPatroling = true;
            targetDirection = Vector2.zero;
            stateMachine.ChangeState(idleState);//待命状态 
        }
    } 


    #endregion

    #region ai面朝向部分
    //改变面朝向
    private void AutoUpdateFacing()
    {
        if (target == null || isDead)
            return;

        // 保持当前比例
        Vector3 currentScale = transform.localScale;
        float originalScaleX = Mathf.Abs(currentScale.x);

        // 比较目标与角色的X坐标判断左右方向
        if (target.position.x > transform.position.x)
        {
            // 目标在右x为正
            transform.localScale = new Vector3(originalScaleX, currentScale.y, currentScale.z);
        }
        else if (target.position.x < transform.position.x)
        {
            // 目标在左x为负
            transform.localScale = new Vector3(-originalScaleX, currentScale.y, currentScale.z);
        }
    }
    #endregion

    #region ai巡逻相关
    //ai巡逻相关
    public IEnumerator Patrol()
    {
        //Debug.Log("巡逻开始");
        while (isPatroling && !isDead)
        {
            float randomPatrolTime = Random.Range(2f, patrolTime);//1~3s巡逻时间
            float pastTime = 0f;
            //随机方向
            Vector2 randomDirection = GetRandomPatrolDirection();
            while (pastTime < randomPatrolTime && isPatroling && !isDead)
            {
                rb.velocity = randomDirection * patrolSpeed;
                pastTime += Time.deltaTime;
                yield return null;
            }
            rb.velocity = Vector2.zero;
            yield return new WaitForSeconds(1f);
        }
        yield return new WaitForSeconds(3f);
    }

    public Vector2 GetRandomPatrolDirection()
    {
        //随机角度
        float randomAngle = Random.Range(0f, Mathf.PI * 2);
        Vector2 randomDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        return randomDirection.normalized;
    } 
    #endregion


    //得到目标的协程（防止报空）
    private IEnumerator GetTarget()
    {
        while(target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                target = player.transform;
            }
            yield return new WaitForSeconds(0.5f);
        }
    }

    //简单随机掉落 20掉落率
    private void DropLoot()
    {
        //Debug.Log("掉落物品调用");
        //Debug.Log(lootPool.lootList.Count);
        if (Random.Range(0, 100) < 20)
        {
            //Debug.Log("成功掉落物品");
            GameObject loot = Instantiate(lootPool.lootList[Random.Range(0, lootPool.lootList.Count - 1)], transform.position, Quaternion.identity);
        }
    }
}
