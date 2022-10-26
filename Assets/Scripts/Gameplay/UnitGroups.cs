using System.Collections.Generic;
using UnityEngine;

public class UnitGroups : MonoBehaviour
{
    public UnitGroupUI[] UIUnitGroups;
    public List<List<PlayerUnit>> SelectionGroups;

    private void Awake()
    {
        SelectionGroups = new List<List<PlayerUnit>>();
        for (int i = 0; i < 10; i++) SelectionGroups.Add(new());
    }

    public void SetUnitGroup(int group, List<PlayerUnit> units)
    {
        SelectionGroups[group].Clear();
        SelectionGroups[group].AddRange(units);
        UIUnitGroups[group].SetGroup(units.Count > 0 ? units[0].unitIcon : null, SelectionGroups[group].Count);
    }
    public void AddUnitGroup(int group, List<PlayerUnit> units)
    {
        foreach (var unit in units)
            if (!SelectionGroups[group].Contains(unit))
                SelectionGroups[group].Add(unit);
        UIUnitGroups[group].SetGroup(units.Count > 0 ? units[0].unitIcon : null, SelectionGroups[group].Count);
    }
}
