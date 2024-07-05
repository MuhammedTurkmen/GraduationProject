using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform _target;

    private Vector3 _targetPos;

    private void LateUpdate()
    {
        _targetPos = _target.position;

        _targetPos.z = transform.position.z;

        transform.position = _targetPos;
    }
}