using UnityEngine;

public class UIAnimation : MonoBehaviour
{
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetParameter(bool state)
    {
        _animator.SetBool("enter", state);
    }
}