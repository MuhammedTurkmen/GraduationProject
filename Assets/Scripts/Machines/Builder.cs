using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Builder : MonoBehaviour
{
    [SerializeField]
    private BuildingTypeSO[] _buildingTypeSOs;

    [SerializeField]
    private GameObject _buildingButton;

    private int _buildIndex;

    private BuildingTypeSO _builderSO;

    private Transform _visualizer, _machineParent;
    private Vector2 _centerPoint, _pressPoint;

    private Directions _dir = Directions.Top;

    private Quaternion targetDirection;

    [SerializeField]
    private InputActionProperty _turnLeft, _turnRight;

    [Header("UI COMPONENTS")]

    public EventSystem eventSystem;
    public GraphicRaycaster raycaster;

    private ActiveButton _activeButton;

    private void Awake()
    {
        _activeButton = GetComponent<ActiveButton>();
        _turnLeft.action.started += TurnLeftAction;
        _turnRight.action.started += TurnRightAction;

        targetDirection = Quaternion.Euler(0, 0, 0);
        _dir = Directions.Right;
    }

    private void Start()
    {
        AddBuildingButtons();
    }

    private void Update()
    {
        if (!_activeButton._active)
            return;

        if (IsPointerOverUIElement())
        {
            if (_visualizer != null)
                Destroy(_visualizer.gameObject);

            return;
        }

        if (_builderSO != _buildingTypeSOs[_buildIndex])
        {
            _builderSO = _buildingTypeSOs[_buildIndex];

            if (_visualizer != null)
                Destroy(_visualizer.gameObject);
        }

        if (_builderSO != null && _visualizer == null)
        {
            _visualizer = Instantiate(_builderSO.Visual, _machineParent);
            _visualizer.transform.position = _pressPoint = Mouse.current.position.value.GetWorldPos();
        }

        if (_visualizer != null)
        {
            _pressPoint = Mouse.current.position.value.GetWorldPos();
            _centerPoint = new Vector2(Mathf.FloorToInt(_pressPoint.x) + 0.5f, Mathf.FloorToInt(_pressPoint.y) + 0.5f);
            _visualizer.position = Vector2.Lerp(_visualizer.position, _centerPoint, Time.deltaTime * 15);
            _visualizer.rotation = targetDirection;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Destroy(_visualizer.gameObject);
            Transform buildingObj = Instantiate(_builderSO.Prefab, _machineParent);
            PlacedObject placedObj = buildingObj.GetComponent<PlacedObject>();
            placedObj.SetGridPosition(new Vector2(_centerPoint.x, _centerPoint.y));
            placedObj.SetRotation(_dir);

            if (buildingObj.TryGetComponent(out ConveyorBelt belt))
            {
                BeltSystem.Instance.AddNewBelt2Path(belt);
                belt.moveForward = true;
            }

            buildingObj.SetPositionAndRotation(_centerPoint, targetDirection);
        }
    }

    private void TurnLeftAction(InputAction.CallbackContext obj)
    {
        if (_visualizer != null && !_visualizer.gameObject.activeInHierarchy)
            return;

        if (!_builderSO.TurnAble)
            return;

        switch (_dir)
        {
            case Directions.Right:
                _dir = Directions.Top;
                break;
            case Directions.Top:
                _dir = Directions.Left;
                break;
            case Directions.Left:
                _dir = Directions.Bottom;
                break;
            case Directions.Bottom:
                _dir = Directions.Right;
                break;
        }

        SetAngle();
    }

    private void TurnRightAction(InputAction.CallbackContext obj)
    {
        if (_visualizer != null && !_visualizer.gameObject.activeInHierarchy)
            return;

        if (!_builderSO.TurnAble)
            return;

        switch (_dir)
        {
            case Directions.Right:
                _dir = Directions.Bottom;
                break;
            case Directions.Bottom:
                _dir = Directions.Left;
                break;
            case Directions.Left:
                _dir = Directions.Top;
                break;
            case Directions.Top:
                _dir = Directions.Right;
                break;
        }

        SetAngle();
    }

    private void SetAngle()
    {
        targetDirection = _dir.Direction2Quaternion();
    }

    public void SetIndex(int i)
    {
        _buildIndex = i;
    }

    private bool IsPointerOverUIElement()
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();

        raycaster.Raycast(eventData, results);

        return results.Count > 0;
    }

    private void AddBuildingButtons()
    {
        for (int i = 0; i < _buildingTypeSOs.Length; i++)
        {
            int a = i;

            GameObject button = Instantiate(_buildingButton, transform.GetChild(1));

            button.GetComponent<Button>().onClick.AddListener(() => SetIndex(a));

            button.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text =
                _buildingTypeSOs[i].BuildingName[0].ToString();
        }
    }
}