using System.Security.Cryptography;
using UnityEngine;

public abstract class State : MonoBehaviour
{
    public bool isComplete;
    protected Core _core;

    protected Rigidbody2D body => _core.body;
    protected PlayerStateController input => _core.stateController;

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Do() { }
    public virtual void FixedDo() { }

    public void DoBranch()
    {
        _core.stateController.currentState?.Do();
    }
    public void FixedDoBranch()
    {
        _core.stateController.currentState?.FixedDo();
    }
    public void SetCore(Core core)
    {
        _core = core;
    }
}