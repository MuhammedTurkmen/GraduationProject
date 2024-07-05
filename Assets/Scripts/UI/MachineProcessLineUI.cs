using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MachineProcessLineUI : MonoBehaviour
{
    public int InputIndex => _inputIndex;
    public int InputIndex2 => _inputIndex2;
    public int ProcessIndex => _processIndex;
    public int OutIndex => _outputIndex;

    private int _inputIndex, _inputIndex2,_processIndex, _outputIndex;
    private AutomaticMachineUI _machineUI;
    private TMP_Dropdown[] _tMP_Dropdowns;

    public ProcessLineType type;

    private void Awake()
    {
        _machineUI = GetComponentInParent<AutomaticMachineUI>();
        _tMP_Dropdowns = GetComponentsInChildren<TMP_Dropdown>();
    }

    public void SetInputIndex(int i)
    {
        _inputIndex = i;
    }

    public void SetInputIndex2(int i)
    {
        _inputIndex2 = i;
    }

    public void SetProcessIndex(int i)
    {
        _processIndex = i;
    }
    
    public void SetOutputIndex(int i)
    {
        _outputIndex = i;


    }

    public void SetDropdownOptions()
    {
        for (int i = 0; i < _tMP_Dropdowns.Length; i++)
        {
            _tMP_Dropdowns[i].options.Clear();
        }

        for (int i = 0; i < _tMP_Dropdowns.Length; i++)
        {
            if (i == 0)
            {
                for (int j = 0; j < _machineUI.Inputs.Count; j++)
                {
                    _tMP_Dropdowns[i].options.Add(new TMP_Dropdown.OptionData() { text = _machineUI.Inputs[j] });
                }
            }
            else if (i == 1)
            {
                for (int j = 0; j < _machineUI.ProcessTypes.Count; j++)
                {
                    _tMP_Dropdowns[i].options.Add(new TMP_Dropdown.OptionData() { text = _machineUI.ProcessTypes[j] });
                }
            }
            else if (i == 2)
            {
                for (int j = 0; j < _machineUI.Outputs.Count; j++)
                {
                    _tMP_Dropdowns[i].options.Add(new TMP_Dropdown.OptionData() { text = _machineUI.Outputs[j] });
                }
            }
        }
    }

    public void SetUIElements(bool state)
    {
        if (type == ProcessLineType.InputInputOutput)
            return;

        transform.GetChild(0).GetChild(0).gameObject.SetActive(state);
        transform.GetChild(1).GetChild(0).gameObject.SetActive(state);
        transform.GetChild(1).GetChild(1).gameObject.SetActive(state);
    }
}