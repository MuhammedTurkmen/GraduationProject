using System;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject MachinePanel;

    public static event EventHandler panelOpened, panelClosed;

    public void ChangeUIState(GameObject UI)
    {
        if (UI.activeInHierarchy)
        {
            panelClosed?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            panelOpened?.Invoke(this, EventArgs.Empty);
        }

        UI.SetActive(!UI.activeInHierarchy); 
    }

    public void OpenMachineUI()
    {
        if (MachinePanel.activeInHierarchy)
        {
            panelClosed?.Invoke(this, new EventArgs { });
        }
        else
        {
            panelOpened?.Invoke(this, new EventArgs { });
        }

        MachinePanel.SetActive(!MachinePanel.activeInHierarchy);

    }

}