using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerHoldReleaseAction : MonoBehaviour
{
    private PlayerStateController stateController;

    private Transform _holdPosition;

    private void Awake()
    {
        _holdPosition = transform.Find("Interact").Find("Object Holder");

        stateController = GetComponent<PlayerStateController>();
    }

    public InputActionProperty HoldAction, ThrowAction;

    private void Start()
    {
        HoldAction.action.started += _ => HoldObject();
        ThrowAction.action.started += _ => ThrowObject();
    }

    private Transform _holdedObject;
    private Rigidbody2D _holdedObjectRigi;
    private BoxCollider2D _holdedObjectColl, _holdedObjectHitboxColl;

    private void HoldObject()
    {
        RaycastHit2D hittedItem =
            Physics2D.BoxCast(
                transform.position,
                new Vector2(stateController._playerStats.HoldCastSizeX, stateController._playerStats.HoldCastSizeY),
                0,
                transform.up,
                stateController._playerStats.HoldDistance,
                stateController._playerStats.ObjectMask
            );

        RaycastHit2D hittedMachine =
            Physics2D.BoxCast(
                transform.position,
                new Vector2(stateController._playerStats.HoldCastSizeX, stateController._playerStats.HoldCastSizeY),
                0,
                transform.up,
                stateController._playerStats.HoldDistance,
                stateController._playerStats.MachineMask
            );

        if (_holdPosition.transform.childCount == 0)
        {
            if (hittedMachine && hittedMachine.collider.TryGetComponent(out CraftingTable ctable))
            {
                if (ctable.ItemsIsPlaced)
                {
                    ctable.ThrowItems();
                    return;
                }
            }

            if (hittedItem)
            {
                _holdedObject = hittedItem.transform;
                Vector2 pos =
                    new Vector2(
                        Mathf.FloorToInt(_holdedObject.position.x) + 0.5f,
                        Mathf.FloorToInt(_holdedObject.position.y) + 0.5f
                    );

                GridSystem.Instance.RemoveItem(pos);
                //print($"Holded OBJ: {_holdedObject.position}");
                _holdedObjectRigi = hittedItem.transform.GetComponent<Rigidbody2D>();
                _holdedObjectColl = hittedItem.transform.GetComponent<BoxCollider2D>();
                _holdedObjectHitboxColl = hittedItem.transform.GetChild(1).GetComponent<BoxCollider2D>();
                _holdedObjectRigi.velocity = Vector2.zero;

                _holdedObjectColl.enabled = false;
                _holdedObjectHitboxColl.enabled = false;

                _holdedObjectRigi.bodyType = RigidbodyType2D.Kinematic;
                _holdedObjectColl.isTrigger = true;

                _holdedObject.parent = _holdPosition.transform;
                _holdedObject.position = _holdPosition.position;
                _holdedObject.rotation = new Quaternion(0, 0, 0, 0);
            }
        }
        else if (_holdPosition.transform.childCount > 0)
        {
            _holdedObjectRigi.bodyType = RigidbodyType2D.Dynamic;
            _holdedObjectColl.enabled = true;
            _holdedObjectHitboxColl.enabled = true;
            _holdedObjectColl.isTrigger = false;

            _holdedObject.parent = null;

            if (_holdedObject.TryGetComponent(out ItemUser item))
            {
                item.CanInteract();
            }
        }
    }

    private void ThrowObject()
    {
        if (_holdedObject == null)
            return;

        if (_holdPosition.childCount > 0)
        {
            _holdedObjectRigi.bodyType = RigidbodyType2D.Dynamic;
            _holdedObjectColl.enabled = true;
            _holdedObjectHitboxColl.enabled = true;
            _holdedObjectColl.isTrigger = false;

            _holdedObjectRigi.velocity = transform.up.normalized * stateController._playerStats.ThrowForce;
            _holdedObject.parent = null;

            if (_holdedObject.TryGetComponent(out ItemUser item))
            {
                item.CanInteract();
            }
        }
    }

}