using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerState
{
    //整个枚举 分辨Animator的参数类型
    public enum E_AnimatorParamType
    {
        Bool,
        Trigger
    }


    protected PlayerStateMachine stateMachine;
    protected Player player;

    private string animatorParamName;
    private E_AnimatorParamType animatorParamType;

    protected float xInput;
    protected float yInput;


    //private int dashCount = 2;
    //private int currentDashCount = 0;
    //private float dashDuration = 0.1f;
    //private float dashTime = 0;

    public PlayerState( Player player, PlayerStateMachine stateMachine, string animatorParamName, E_AnimatorParamType animatorParamType)
    {
        this.player = player;
        this.stateMachine = stateMachine;
        this.animatorParamName = animatorParamName;
        this.animatorParamType = animatorParamType;
    }

    public virtual void Enter()
    {
        switch (animatorParamType)
        {
            case E_AnimatorParamType.Bool:
                player.animator.SetBool(animatorParamName, true);
                break;
            case E_AnimatorParamType.Trigger:
                player.animator.SetTrigger(animatorParamName);
                break;
        }

    }

    public virtual void Update()
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");

        if (Input.GetMouseButtonDown(0))
            stateMachine.ChangeState(player.lightAttack);
        if (Input.GetMouseButtonDown(1))
            stateMachine.ChangeState(player.heavyAttack);

        //dashTime += Time.deltaTime;
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    Dash();
        //}
    }

    //public void Dash()
    //{
    //    if (currentDashCount >= dashCount)
    //        return;//这里播放冷却语音
    //    //移动
    //    dashTime = 0;
    //    if (dashTime <= dashDuration)
    //    {
    //        player.SetVelocity(xInput * player.moveSpeed*10, yInput * player.moveSpeed*10);
    //    }
    //    currentDashCount++;

    //}
    //public IEnumerator DashCoroutine()
    //{
    //    yield return new WaitForSeconds(3f);
    //    currentDashCount--;
    //}

    public virtual void Exit()
    {
        switch (animatorParamType)
        {
            case E_AnimatorParamType.Bool:
                player.animator.SetBool(animatorParamName, false);
                break;
            case E_AnimatorParamType.Trigger: //trigger不用退出
                break;
        }

    }


}
