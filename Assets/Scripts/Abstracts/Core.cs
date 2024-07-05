using UnityEngine;

public abstract class Core : MonoBehaviour
{
    public PlayerActions lastState;
    public PlayerStateController stateController;
    public Rigidbody2D body;

    public void SetupInstances()
    {
        State[] states = GetComponentsInChildren<State>();

        foreach (State state in states)
        {
            state.SetCore(this);
        }
    }
}