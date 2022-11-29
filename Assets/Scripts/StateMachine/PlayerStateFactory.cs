public class PlayerStateFactory
{
    PlayerStateMachine _context;
    public PlayerStateFactory(PlayerStateMachine currentContext)
    {
        _context = currentContext;
    }

    public PlayerBaseState Idle()
    {
        return new PlayerIdleState(_context, this);
    }
    public PlayerBaseState Walk()
    {
        return new PlayerWalkState(_context, this);
    }
    public PlayerBaseState Dash()
    {
        return new PlayerDashState(_context, this);
    }
}
