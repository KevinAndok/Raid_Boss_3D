using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public UnitGroups UnitGroups;
    public List<PlayerUnit> selectedUnits = new List<PlayerUnit>();

    public Color selectedColor;

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

        CustomInput.Instance.OnTabDown      += SwitchSelectedUnit;
    }

    private void Update()
    {
        DisplayCurrentlyUsedUnit();
    }

    private void DisplayCurrentlyUsedUnit()
    {
        foreach (var unit in selectedUnits)
        {
            ColorCirclesBasedonSelection(unit);
        }
    }

    private void ColorCirclesBasedonSelection(PlayerUnit unit)
    {
        if (!CustomInput.Instance.altDown || true)
        {
            if (unit == selectedUnits[0]) unit.selectionCircleGFX.Color = selectedColor;
            else unit.selectionCircleGFX.Color = Color.white;
            return;
        }
        unit.selectionCircleGFX.Color = selectedColor;
    }

    public void UnselectAllUnits()
    {
        foreach (PlayerUnit e in selectedUnits) e.selectionCircle.SetActive(false);
        selectedUnits.Clear();
    }

    public void SelectUnit(PlayerUnit unit)
    {
        unit.selectionCircle.SetActive(true);
        selectedUnits.Add(unit);
    }
    public void UnselectUnit(PlayerUnit unit)
    {
        unit.selectionCircle.SetActive(false);
        selectedUnits.Remove(unit);
    }

    public void SelectionGroup(int number)
    {
        if (CustomInput.Instance.shiftDown)
        {
            UnitGroups.AddUnitGroup(number, selectedUnits);
            return;
        }
        if (CustomInput.Instance.ctrlDown || CustomInput.Instance.altDown)
        {
            UnitGroups.SetUnitGroup(number, selectedUnits);
            return;
        }
        //select units
        UnselectAllUnits();
        foreach (var unit in UnitGroups.SelectionGroups[number]) SelectUnit(unit);
    }

    public void SwitchSelectedUnit()
    {
        if (selectedUnits.Count < 2) return;

        var unit = selectedUnits[0];
        selectedUnits.RemoveAt(0);
        selectedUnits.Add(unit);
    }
}
