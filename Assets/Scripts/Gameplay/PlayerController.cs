using System.Collections.Generic;
using UnityEngine;

public sealed class PlayerController : MonoBehaviour
{
    public UnitGroups UnitGroups;
    public List<PlayerUnit> selectedUnits = new List<PlayerUnit>();

    public Color selectedColor;

    #region InputEventsRegister
    private void OnEnable()
    {
        CustomInput.OnZeroDown += () => SelectionGroup(0);
        CustomInput.OnOneDown += () => SelectionGroup(1);
        CustomInput.OnTwoDown += () => SelectionGroup(2);
        CustomInput.OnThreeDown += () => SelectionGroup(3);
        CustomInput.OnFourDown += () => SelectionGroup(4);
        CustomInput.OnFiveDown += () => SelectionGroup(5);
        CustomInput.OnSixDown += () => SelectionGroup(6);
        CustomInput.OnSevenDown += () => SelectionGroup(7);
        CustomInput.OnEightDown += () => SelectionGroup(8);
        CustomInput.OnNineDown += () => SelectionGroup(9);

        CustomInput.OnTabDown += SwitchSelectedUnit;
    }
    private void OnDisable()
    {
        CustomInput.OnZeroDown -= () => SelectionGroup(0);
        CustomInput.OnOneDown -= () => SelectionGroup(1);
        CustomInput.OnTwoDown -= () => SelectionGroup(2);
        CustomInput.OnThreeDown -= () => SelectionGroup(3);
        CustomInput.OnFourDown -= () => SelectionGroup(4);
        CustomInput.OnFiveDown -= () => SelectionGroup(5);
        CustomInput.OnSixDown -= () => SelectionGroup(6);
        CustomInput.OnSevenDown -= () => SelectionGroup(7);
        CustomInput.OnEightDown -= () => SelectionGroup(8);
        CustomInput.OnNineDown -= () => SelectionGroup(9);

        CustomInput.OnTabDown -= SwitchSelectedUnit;
    }
    #endregion

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
        if (!CustomInput.altDown /*|| true*/)
        {
            if (unit == selectedUnits[0])
            {
                unit.selectionCircleGFX.Color = selectedColor;
                unit.DisplayCommands();
            }
            else
            {
                unit.selectionCircleGFX.Color = Color.white;
                unit.HideCommands();

            }
            return;
        }
        unit.selectionCircleGFX.Color = selectedColor;
        unit.DisplayCommands();
    }

    public void DeselectAllUnits()
    {
        foreach (PlayerUnit e in selectedUnits)
        {
            e.selectionCircle.SetActive(false);
            e.HideCommands();
        }
        selectedUnits.Clear();
    }

    public void SelectUnit(PlayerUnit unit)
    {
        if (selectedUnits.Contains(unit)) return;
        unit.selectionCircle.SetActive(true);
        selectedUnits.Add(unit);
    }
    public void DeselectUnit(PlayerUnit unit)
    {
        unit.selectionCircle.SetActive(false);
        selectedUnits.Remove(unit);
        unit.HideCommands();
    }

    public void SelectionGroup(int number)
    {
        if (CustomInput.shiftDown)
        {
            UnitGroups.AddUnitGroup(number, selectedUnits);
            return;
        }
        if (CustomInput.ctrlDown || CustomInput.altDown)
        {
            UnitGroups.SetUnitGroup(number, selectedUnits);
            return;
        }
        //select units
        DeselectAllUnits();
        foreach (var unit in UnitGroups.SelectionGroups[number]) SelectUnit(unit);
    }

    public void SwitchSelectedUnit()
    {
        if (selectedUnits.Count < 2) return;

        var unit = selectedUnits[0];
        selectedUnits.RemoveAt(0);
        selectedUnits.Add(unit);

        unit.HideCommands();
    }
}
