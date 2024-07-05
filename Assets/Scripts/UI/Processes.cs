using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Processes : MonoBehaviour
{
    public GameObject inputInputOutputLine, inputProcessOutputLine;

    public MachineProcessLineUI[] machineProcessLineUIs;

    public AutomaticMachineUI machine;

    private void Start()
    {
        machine = transform.GetComponentInParent<AutomaticMachineUI>();

        MenuManager.panelClosed += SetProcessLines;
    }

    private void SetProcessLines(object sender, EventArgs e)
    {
        machineProcessLineUIs = transform.GetComponentsInChildren<MachineProcessLineUI>();


    }

    public void CallCreateProcessLine(GameObject prefab)
    {
        StartCoroutine(CreateProcessLine(prefab));
    }

    public IEnumerator CreateProcessLine(GameObject prefab)
    {
        GameObject line = Instantiate(prefab, transform);

        int index = line.transform.GetSiblingIndex();

        TMP_Dropdown dropdown =
            line.transform.GetChild(1).GetChild(1).GetChild(4).GetComponent<TMP_Dropdown>();

        dropdown.onValueChanged.AddListener(
                delegate { CloseInputSections(dropdown.value); }
            ) ;

        line.transform.GetChild(2).GetComponent<Button>()
            .onClick.AddListener(()=> CallRemoveLine(line));


        line.transform.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text 
            = $"{index}.";

        line.transform.SetSiblingIndex(line.transform.parent.childCount - 2);

        line.SetActive(true);

        yield return null;
        
        machine.Lines.Add("SATIR " + index.ToString());
        SetAllLineDropdowns();
    }

    private void SetAllLineDropdowns()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            machine.SetProcessLine(transform.GetChild(i));
        }
    }

    public void CloseInputSections(int index)
    {
        if (index <= 1)
            return;

        transform.GetChild(index - 2).GetComponentInChildren<MachineProcessLineUI>().SetUIElements(false);
    }

    public void ReNumarateLines()
    {
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            transform.GetChild(i).transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text
                = $"{i + 1}.";
        }
    }

    public void CallRemoveLine(GameObject line)
    {
        StartCoroutine(RemoveLine(line));
    }

    public IEnumerator RemoveLine(GameObject line)
    {
        Destroy(line);
        yield return null;
        ReNumarateLines();
    }
}