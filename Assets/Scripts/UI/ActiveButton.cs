using UnityEngine;
using UnityEngine.UI;

public class ActiveButton : MonoBehaviour
{
    [Header("ACTIVE BUTTON")]
    internal bool _active;

    public Color ActiveColor;

    private Image _button;

    private void Awake()
    {
        _button = transform.GetComponent<Image>();
    }

    public void SetState(bool state)
    {
        _active = state;

        SetColor();
    }
   

    public void ChangeState()
    {
        _active = !_active;

        SetColor();
    }

    public void SetColor()
    {
        if (_button == null)
        {
            print("Boþ"); return;
        }
        if (_active)
        {
            _button.color = ActiveColor;
        }
        else
        {
            _button.color = Color.white;
        }
    }
}