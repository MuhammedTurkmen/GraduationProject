using UnityEngine.InputSystem;
using UnityEngine;

public class PlayerStateController : Core
{
    [Header("STATS")]
    public PlayerStatsSO _playerStats;

    #region STATES

    public State currentState;
    
    public State WalkState;
    public State DashState;

    public bool isDashButtonClicked; 

    #endregion

    #region INPUT ACTION

    [Header("INPUTS")]
    
    public InputActionAsset PlayerInputs;

    public InputActionProperty MoveAction;
    public InputActionProperty DashAction;

    #endregion


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
        stateController = this;
        body = GetComponent<Rigidbody2D>();

        currentState = WalkState;

        SetupInstances();
    }

    private void Start()
    {
        DashAction.action.started += _ => ActivateDash();
    }

    private void Update()
    {
        if (currentState != null)
            currentState.DoBranch();
    }

    private void FixedUpdate()
    {
        HandleInputs();

        SelectState();

        if (currentState != null)
            currentState.FixedDoBranch();
    }

    private void SelectState()
    {
        if (currentState == WalkState && isDashButtonClicked)
        {
            if (currentState.isComplete)
            {
                currentState = DashState;
                isDashButtonClicked = false;
            }
            currentState.Enter();
        }
        else if (currentState == DashState && currentState.isComplete)
        {
            currentState = WalkState;
            currentState.Enter();
        }
    }

    #region STARTING

    private void ActivateDash()
    {
        if (currentState != DashState && currentState.isComplete)
        {
            isDashButtonClicked = true;
        }
    }

    #endregion

    #region INPUTS

    public float _xSpeed, _ySpeed;
    public Vector2 _velocity;
    public Vector2 _input;
    public Vector2 _dashDirection;
    public float _dashSpeed;

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