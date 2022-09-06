using System.Collections.Generic;
using UnityEngine;

public class UnitGroups : MonoBehaviour
{
    public List<List<Entity>> SelectionGroups;

    private void Awake()
    {
        SelectionGroups = new List<List<Entity>>();
        for (int i = 0; i < 10; i++) SelectionGroups.Add(new());
    }

    public void SetUnitGroup(int group, List<Entity> units)
    {
        SelectionGroups[group].Clear();
        SelectionGroups[group].AddRange(units);
    }
    public void AddUnitGroup(int group, List<Entity> units)
    {
        foreach (var unit in units) 
            if (!SelectionGroups[group].Contains(unit)) 
                SelectionGroups[group].Add(unit);
    }
}
