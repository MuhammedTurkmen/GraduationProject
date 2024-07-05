using System.Collections.Generic;
using UnityEngine;

public class DiagramPanelController : MonoBehaviour
{
    // input - output
    public List<InputItemUI> inputItemUIs;

    // process types
    public Dictionary<int, ProcessTypeIconUI> processItemUIs;
    public Dictionary<int, ProcessTypeIconUI> processItemUIs2;

    private void Start()
    {
        inputItemUIs.Add(transform.GetChild(1).GetChild(0).GetComponent<InputItemUI>());
        inputItemUIs.Add(transform.GetChild(1).GetChild(1).GetComponent<InputItemUI>());
    }

    public void TakeDiagrams()
    {
        //inputItemUIs[0].connectedProcess
    }
}