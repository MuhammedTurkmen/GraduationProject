using System.Collections.Generic;
using UnityEngine;

public class AutomaticMachineConveyorBeltController : MonoBehaviour
{
    private List<ConveyorBelt> belts;

    private void Awake()
    {
        belts = new();
    }

    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            belts.Add(transform.GetChild(i).GetComponent<ConveyorBelt>());
        }

        for (int i = 0; i < belts.Count; i++)
        {
            belts[i].SetOrigin();
        }

        TimeTickSystem.OnTick += delegate (object sender, TimeTickSystem.OnTickEventArgs e)
        {
            OperateBelts();
        };
    }

    public void OperateBelts()
    {
        if (belts.Count <= 0)
        {
            print("yeterli bant yok");
            return;
        }

        for (int i = 0; i < belts.Count; i++)
        {
            belts[i].TakeAction(false);
        }
    }
}