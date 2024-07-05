using UnityEngine;

[CreateAssetMenu(fileName = "New Stats", menuName = "Character Stats/Stats")]
public class PlayerStatsSO : ScriptableObject
{
    #region INPUTS

    [Header("INPUTS")]

    [SerializeField]
    private bool _snapInput = true;

    [SerializeField, Range(0.01F, 0.99F)]
    private float _verticalDeadZoneThreshold = 0.3f;

    [SerializeField, Range(0.01F, 0.99F)]
    private float _horizontalDeadZoneThreshold = 0.1f;

    #endregion

    #region WALKING

    [Header("WALKING")]

    [SerializeField]
    private float _maxWalkSpeed = 6;

    [SerializeField]
    private float _acceleration = 10;
    [SerializeField]
    private float _deceleration = 20;
    [SerializeField]
    private float _maxDashSpeed = 60;
    [SerializeField]
    private float _minDashSpeed = 20;
    [SerializeField]
    private float _dashDeceleration = 10;

    #endregion

    #region THROWING

    [Header("THROWING")]

    [SerializeField, Range(1f, 30f)]
    private float _throwForce;

    #endregion

    #region HOLDING 

    [Header("HOLDING")]

    [SerializeField]
    private LayerMask _objectMask;
    [SerializeField]
    private LayerMask _machineMask;

    [SerializeField]
    private float _holdDistance, _holdCastSizeX, _holdCastSizeY;

    #endregion

    #region PROPERTIES

    public bool SnapInput { get { return _snapInput; } }
    public float VerticalDeadZoneThreshold { get { return _verticalDeadZoneThreshold; } }
    public float HorizontalDeadZoneThreshold { get { return _horizontalDeadZoneThreshold; } }

    public float MaxWalkSpeed { get { return _maxWalkSpeed; } }
    public float Acceleration { get { return _acceleration; } }
    public float Deceleration { get { return _deceleration; } }
    public float MaxDashSpeed { get { return _maxDashSpeed; } }
    public float MinDashSpeed { get { return _minDashSpeed; } }
    public float DashDeceleration { get { return _dashDeceleration; } }


    public float ThrowForce { get { return _throwForce; } }

    public LayerMask ObjectMask { get { return _objectMask; } }
    public LayerMask MachineMask { get { return _machineMask; } }
    public float HoldDistance { get { return _holdDistance; } }
    public float HoldCastSizeX { get { return _holdCastSizeX; } }
    public float HoldCastSizeY { get { return _holdCastSizeY; } }

    #endregion
}
