using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterState
{
    public enum E_AnimatorParamType
    {
        Bool,
        Trigger
    }

    protected MonsterStateMachine stateMachine;
    protected Monster monster;

    private string animatorParamName;
    private E_AnimatorParamType animatorParamType;


    public MonsterState(Monster monster,MonsterStateMachine stateMachine,  string animatorParamName, E_AnimatorParamType animatorParamType)
    {
        this.stateMachine = stateMachine;
        this.monster = monster;
        this.animatorParamName = animatorParamName;
        this.animatorParamType = animatorParamType;
    }

    public virtual void Enter()
    {
        switch (animatorParamType)
        {
            case E_AnimatorParamType.Bool:
                monster.animator.SetBool(animatorParamName, true);
                break;
            case E_AnimatorParamType.Trigger:
                monster.animator.SetTrigger(animatorParamName);
                break;
        }
    }

    public virtual void Update()
    {

    }
    public virtual void Exit()
    {
        switch (animatorParamType)
        {
            case E_AnimatorParamType.Bool:
                monster.animator.SetBool(animatorParamName, false);
                break;
            case E_AnimatorParamType.Trigger: //trigger²»ÓÃÍË³ö
                break;
        }
    }

   
}
