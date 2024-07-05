using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AutomaticMachineUI : MonoBehaviour
{
    public List<string> Inputs = new();
    public List<string> Outputs = new();
    public List<string> ProcessTypes = new();
    public List<string> Lines = new();

    public TMP_Dropdown inputDropdown, inputDropdown2, processDropdown, outputDropdown;

    private void Awake()
    {
        //inputDropdown = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>();
        //processDropdown = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(2).GetComponent<TMP_Dropdown>();
        //outputDropdown = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(4).GetComponent<TMP_Dropdown>();

        //inputDropdown.ClearOptions();
        //inputDropdown.AddOptions(Inputs);

        //processDropdown.ClearOptions();
        //processDropdown.AddOptions(ProcessTypes);

        //outputDropdown.ClearOptions();
        //outputDropdown.AddOptions(Outputs);
    }

    public void SetProcessLine(Transform processLine)
    {
        if (processLine.GetChild(1).TryGetComponent(out MachineProcessLineUI line))
        {
            if (line.type == ProcessLineType.InputInputOutput)
            {
                inputDropdown = processLine.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>();
                inputDropdown2 = processLine.GetChild(1).GetChild(1).GetChild(2).GetComponent<TMP_Dropdown>();
                outputDropdown = processLine.GetChild(1).GetChild(1).GetChild(4).GetComponent<TMP_Dropdown>();

                inputDropdown.ClearOptions();
                inputDropdown.AddOptions(Inputs);
                inputDropdown.AddOptions(Lines);

                inputDropdown2.ClearOptions();
                inputDropdown2.AddOptions(Inputs);
                inputDropdown2.AddOptions(Lines);

                outputDropdown.ClearOptions();
                outputDropdown.AddOptions(Outputs);
                outputDropdown.AddOptions(Lines);
            }
            else if (line.type == ProcessLineType.InputProcessOutput)
            {
                inputDropdown = processLine.GetChild(1).GetChild(1).GetChild(0).GetComponent<TMP_Dropdown>();
                processDropdown = processLine.GetChild(1).GetChild(1).GetChild(2).GetComponent<TMP_Dropdown>();
                outputDropdown = processLine.GetChild(1).GetChild(1).GetChild(4).GetComponent<TMP_Dropdown>();

                inputDropdown.ClearOptions();
                inputDropdown.AddOptions(Inputs);
                inputDropdown.AddOptions(Lines);

                processDropdown.ClearOptions();
                processDropdown.AddOptions(ProcessTypes);

                outputDropdown.ClearOptions();
                outputDropdown.AddOptions(Outputs);
                outputDropdown.AddOptions(Lines);
            }
        }
    }
}