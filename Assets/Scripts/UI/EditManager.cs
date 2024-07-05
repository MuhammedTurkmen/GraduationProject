using UnityEngine;
using UnityEngine.InputSystem;

public class EditManager : MonoBehaviour
{
    private GameObject _gameObject;

    [SerializeField]
    private LayerMask _mask;

    private Camera _mainCam;

    private ActiveButton _activeButton;

    [SerializeField]
    private Color _destroyableColor;

    private Color _normalColor = Color.white;

    private void Awake()
    {
        _activeButton = GetComponent<ActiveButton>();
        _mainCam = Camera.main;
    }

    private Transform _destroyableObject;

    private void Update()
    {
        if (_activeButton._active)
        {
            Vector3 mousePos =
                _mainCam.ScreenToWorldPoint(Mouse.current.position.value);

            RaycastHit2D ray = Physics2D.Raycast(mousePos, Vector3.forward, Mathf.Infinity, _mask);

            if (!ray && _destroyableObject != null)
            {
                SpriteRenderer sprite = _destroyableObject.GetComponentInChildren<SpriteRenderer>();
                sprite.color = _normalColor;
                _destroyableObject = null;
            }

            if (ray)
            {
                if (_destroyableObject == null)
                {
                    _destroyableObject = ray.collider.transform;
                }

                if (_destroyableObject != ray.collider.transform)
                {
                    SpriteRenderer sprite = _destroyableObject.GetComponentInChildren<SpriteRenderer>();
                    sprite.color = _normalColor;
                    _destroyableObject = ray.collider.transform;
                }

                if (Mouse.current.leftButton.wasPressedThisFrame)
                {
                    if (ray.collider.GetComponent<ConveyorBelt>())
                    {
                        BeltSystem.Instance.RemoveBeltFromPath(ray.collider.transform.position);
                    }

                    Destroy(ray.collider.gameObject);
                }
                else if (_destroyableObject != null)
                {
                    SpriteRenderer sprite = _destroyableObject.GetComponentInChildren<SpriteRenderer>();
                    sprite.color = _destroyableColor;
                }
            }
        }
    }
}