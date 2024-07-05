using UnityEngine.InputSystem;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class PlayerController : MonoBehaviour
{
    [Header("STATS")]

    [SerializeField]
    private PlayerStatsSO _playerStats;

    #region OBJECTS

    private Transform _holdPosition;

    #endregion

    #region INPUT ACTION

    [Header("INPUTS")]
    
    public InputActionAsset PlayerInputs;

    public InputActionProperty MoveAction;

    public InputActionProperty HoldAction, ThrowAction, UseAction;

    public InputActionProperty DashAction;

    #endregion

    #region COMPONENTS

    private Rigidbody2D _rigi;

    #endregion

    #region STARTING

    /// <summary>
    /// Nesne aktif olduðunda çalýþan Unity Event'idir
    /// </summary>
    private void OnEnable()
    {
        PlayerInputs.Enable();
    }

    /// <summary>
    /// Nesne deaktif olduðunda çalýþan Unity Event'idir
    /// </summary>
    private void OnDisable()
    {
        PlayerInputs.Disable();
    }

    private void Awake()
    {
        _holdPosition = transform.Find("Interact").Find("Object Holder");

        _rigi = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // 'HoldAction' aksiyonu gerçekleþtiðinde 'HoldObject' fonksiyonunu çaðýrýr
        // Bu actionlar bir 'Button' actionudur
        HoldAction.action.started += _ => HoldObject();
        ThrowAction.action.started += _ => ThrowObject();
        UseAction.action.started += _ => UseMachine();
    }

    #endregion

    private void FixedUpdate()
    {
        HandleInputs();

        Movement();

        Dash();

        ApplyMovement();
    }

    #region INPUTS

    private Vector2 _input;

    private void HandleInputs()
    {
        _input = MoveAction.action.ReadValue<Vector2>();

        if (_playerStats.SnapInput)
        {
            if (_input.x > _playerStats.HorizontalDeadZoneThreshold)
                _input.x = 1;
            else if (_input.x < -_playerStats.HorizontalDeadZoneThreshold)
                _input.x = -1;
            else
                _input.x = 0;

            if (_input.y > _playerStats.VerticalDeadZoneThreshold)
                _input.y = 1;
            else if (_input.y < -_playerStats.VerticalDeadZoneThreshold)
                _input.y = -1;
            else
                _input.y = 0;
        }

        if (_input != Vector2.zero)
        {
            Quaternion TargetRotation =
                Quaternion.LookRotation(transform.forward, new Vector2(_input.x, _input.y));

            transform.rotation = TargetRotation;
        }
    }

    #endregion

    #region MOVEMENT

    private float _xSpeed, _ySpeed;
    private Vector2 _velocity;
    private MovementState _state;

    private void Movement()
    {
        if (_state != MovementState.Walking)
            return;

        if (_input.x != 0)
            _xSpeed = Mathf.MoveTowards(_xSpeed, _input.x * _playerStats.MaxWalkSpeed, _playerStats.Acceleration * Time.fixedDeltaTime);
        else
            _xSpeed = Mathf.MoveTowards(_xSpeed, 0, _playerStats.Deceleration * Time.fixedDeltaTime);

        if (_input.y != 0)
            _ySpeed = Mathf.MoveTowards(_ySpeed, _input.y * _playerStats.MaxWalkSpeed, _playerStats.Acceleration * Time.fixedDeltaTime);
        else
            _ySpeed = Mathf.MoveTowards(_ySpeed, 0, _playerStats.Deceleration * Time.fixedDeltaTime);

        _velocity = new Vector2(_xSpeed, _ySpeed);
    }

    private Vector2 _dashDirection;
    private float _dashSpeed;

    private void Dash()
    {
        if (_state != MovementState.Dash && DashAction.action.IsPressed())
        {
            _state = MovementState.Dash;
            _dashDirection = transform.up;
            _dashSpeed = _playerStats.MaxDashSpeed;
        }

        if (_state == MovementState.Dash)
        {
            _dashSpeed -= _playerStats.DashDeceleration * Time.fixedDeltaTime;

            _velocity = _dashDirection * _dashSpeed;

            if (_dashSpeed <= _playerStats.MinDashSpeed)
            {
                _state = MovementState.Walking;
            }
        }
    }

    #endregion

    private void ApplyMovement() => _rigi.velocity = _velocity;

    #region HOLD / RELEASE / USE

    private Transform _holdedObject;
    private Rigidbody2D _holdedObjectRigi;
    private BoxCollider2D _holdedObjectColl, _holdedObjectHitboxColl;

    private void HoldObject()
    {
        RaycastHit2D hittedItem =
            Physics2D.BoxCast(
                transform.position,
                new Vector2(_playerStats.HoldCastSizeX, _playerStats.HoldCastSizeY),
                0,
                transform.up,
                _playerStats.HoldDistance,
                _playerStats.ObjectMask
            );

        RaycastHit2D hittedMachine =
            Physics2D.BoxCast(
                transform.position,
                new Vector2(_playerStats.HoldCastSizeX, _playerStats.HoldCastSizeY),
                0,
                transform.up,
                _playerStats.HoldDistance,
                _playerStats.MachineMask
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
                print($"Holded OBJ: {_holdedObject.position}");
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

            _holdedObjectRigi.velocity = transform.up.normalized * _playerStats.ThrowForce;
            _holdedObject.parent = null;

            if (_holdedObject.TryGetComponent(out ItemUser item))
            {
                item.CanInteract();
            }
        }
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
                new Vector2(_playerStats.HoldCastSizeX, _playerStats.HoldCastSizeY),
                0,
                transform.up,
                _playerStats.HoldDistance,
                _playerStats.MachineMask
            );

            if (hit.collider != null && hit.collider.TryGetComponent(out CraftingTable table))
            {
                table.TakeItem();
            }
        }
    }

    #endregion

    #region GIZMOS

    public Color DebugRayColor;

    private void OnDrawGizmos()
    {
        Gizmos.color = DebugRayColor;

        // Karakterin Tutmak Ýçin Kullandýðý Iþýný Çizdirir
        Gizmos.DrawCube(
            transform.position + new Vector3(0, _playerStats.HoldDistance / 2 + _playerStats.HoldCastSizeY, 0),
            new Vector2(_playerStats.HoldCastSizeX, -_playerStats.HoldDistance)
        );
    }

    #endregion
}
public enum MovementState
{
    Walking,
    Dash
}