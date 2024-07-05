using UnityEngine;
using UnityEngine.Windows;

public class PlayerMovementState : State
{
    public override void Enter()
    {
        isComplete = false;
    }

    public override void FixedDo()
    {
        isComplete = true;

        if (_core.stateController._input.x != 0)
             _core.stateController._xSpeed = 
                Mathf.MoveTowards(_core.stateController._xSpeed, _core.stateController._input.x * _core.stateController._playerStats.MaxWalkSpeed, _core.stateController._playerStats.Acceleration * Time.fixedDeltaTime);
        else 
            _core.stateController._xSpeed = 
                Mathf.MoveTowards(_core.stateController._xSpeed, 0, _core.stateController._playerStats.Deceleration * Time.fixedDeltaTime);

        if (_core.stateController._input.y != 0)
            _core.stateController._ySpeed = Mathf.MoveTowards(_core.stateController._ySpeed, _core.stateController._input.y * _core.stateController._playerStats.MaxWalkSpeed, _core.stateController._playerStats.Acceleration * Time.fixedDeltaTime);
        else
            _core.stateController._ySpeed = 
                Mathf.MoveTowards(_core.stateController._ySpeed, 0, _core.stateController._playerStats.Deceleration * Time.fixedDeltaTime);

        _core.stateController._velocity = new Vector2(_core.stateController._xSpeed, _core.stateController._ySpeed);

        body.velocity = _core.stateController._velocity;
    }
}