using UnityEngine;

public class MachineClick : MonoBehaviour
{
    MenuManager menuManager;

    private void Awake()
    {
        menuManager = FindObjectOfType<MenuManager>();
    }

    private void OnMouseDown()
    {
        menuManager.OpenMachineUI();
    }
}