using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUseAction : MonoBehaviour
{
    public InputActionProperty UseAction;

    private PlayerStateController stateController;

    private void Awake()
    {
        stateController = GetComponent<PlayerStateController>();
    }

    private void Start()
    {
        UseAction.action.started += _ => UseMachine();
    }

    /// <summary>
    /// Karakterin önünde bir kutu þeklinde ýþýn oluþturur ve önünde bir makine var ise onu kullanýr
    /// </summary>
    private void UseMachine()
    {
        if (UseAction.action.IsPressed())
        {
            RaycastHit2D hit =
            Physics2D.BoxCast(
                transform.position,
                new Vector2(stateController._playerStats.HoldCastSizeX, stateController._playerStats.HoldCastSizeY),
                0,
                transform.up,
                stateController._playerStats.HoldDistance,
                stateController._playerStats.MachineMask
            );

            if (hit.collider != null && hit.collider.TryGetComponent(out CraftingTable table))
            {
                table.TakeItem();
            }
        }
    }
}