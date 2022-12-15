using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWalkState : PlayerBaseState
{
    public PlayerWalkState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
        Debug.Log("Walk");
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState(){}
    public override void InitializeSubState(){}
    public override void CheckSwitchState()
    {
        if (Ctx.MovementInput == new Vector2(0, 0))
        {
            SwitchState(Factory.Idle());
        }

        if (Ctx.IsDashing)
        {
            SwitchState(Factory.Dash());
        }
    }
}