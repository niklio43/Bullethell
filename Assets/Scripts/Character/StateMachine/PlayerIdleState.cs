using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    public PlayerIdleState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
        : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
        Debug.Log("idle");
    }
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void ExitState(){}
    public override void InitializeSubState(){}
    public override void CheckSwitchState()
    {
        if (Ctx.MovementInput != new Vector2(0, 0) && !Ctx.IsDashing)
        {
            SwitchState(Factory.Walk());
        }

        if (Ctx.IsDashing)
        {
            SwitchState(Factory.Dash());
        }
    }
}
