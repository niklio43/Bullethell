using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerBaseState
{
    public PlayerDashState(PlayerStateMachine currentContext, PlayerStateFactory playerStateFactory)
    : base(currentContext, playerStateFactory){}

    public override void EnterState()
    {
        Debug.Log("Dash");
    }
    public override void ExitState(){}
    public override void UpdateState()
    {
        CheckSwitchState();
    }
    public override void InitializeSubState(){}
    public override void CheckSwitchState()
    {
        if (Ctx.IsDashing) return;

        if (Ctx.MovementInput != new Vector2(0, 0))
        {
            SwitchState(Factory.Walk());
        }
        if (Ctx.MovementInput == new Vector2(0, 0))
        {
            SwitchState(Factory.Idle());
        }
    }
}
