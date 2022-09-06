using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public UnitGroups UnitGroups;
    public List<Entity> selectedUnits = new List<Entity>();

    private void Awake()
    {
        CustomInput.Instance.OnZeroDown     += () => SelectionGroup(0);
        CustomInput.Instance.OnOneDown      += () => SelectionGroup(1);
        CustomInput.Instance.OnTwoDown      += () => SelectionGroup(2);
        CustomInput.Instance.OnThreeDown    += () => SelectionGroup(3);
        CustomInput.Instance.OnFourDown     += () => SelectionGroup(4);
        CustomInput.Instance.OnFiveDown     += () => SelectionGroup(5);
        CustomInput.Instance.OnSixDown      += () => SelectionGroup(6);
        CustomInput.Instance.OnSevenDown    += () => SelectionGroup(7);
        CustomInput.Instance.OnEightDown    += () => SelectionGroup(8);
        CustomInput.Instance.OnNineDown     += () => SelectionGroup(9);
    }

    public void UnselectAllUnits()
    {
        foreach (Entity e in selectedUnits) e.selectionCircle.SetActive(false);
        selectedUnits.Clear();
    }

    public void SelectUnit(Entity unit)
    {
        unit.selectionCircle.SetActive(true);
        selectedUnits.Add(unit);
    }

    public void SelectionGroup(int number)
    {
        Debug.Log("Helo");

        if (CustomInput.Instance.shiftDown)
        {
            Debug.Log("shift Helo");
            UnitGroups.AddUnitGroup(number, selectedUnits);
            return;
        }
        if (CustomInput.Instance.ctrlDown || CustomInput.Instance.altDown)
        {
            Debug.Log("ctrl Helo");
            UnitGroups.SetUnitGroup(number, selectedUnits);
            return;
        }
        //select units
        Debug.Log("select Helo");
        UnselectAllUnits();
        foreach (var unit in UnitGroups.SelectionGroups[number]) SelectUnit(unit);
    }
}
