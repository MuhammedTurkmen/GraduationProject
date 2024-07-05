using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDashState : State
{
    public override void Enter()
    {
        _core.stateController._dashSpeed = _core.stateController._playerStats.MaxDashSpeed;
        _core.stateController._dashDirection = transform.up;
        isComplete = false;
    }

    public override void FixedDo()
    {
        
        _core.stateController._dashSpeed -= _core.stateController._playerStats.DashDeceleration * Time.fixedDeltaTime;

        _core.stateController._velocity = _core.stateController._dashDirection * _core.stateController._dashSpeed;

        body.velocity = _core.stateController._velocity;

        if (_core.stateController._dashSpeed <= _core.stateController._playerStats.MinDashSpeed)
        {
            isComplete = true;
        }
    }

    public override void Exit()
    {
        isComplete = false;
    }
}