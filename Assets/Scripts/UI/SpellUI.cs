using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellUI : MonoBehaviour
{
    public SpellIconUI[] spellIcons;

    private void Awake()
    {
        SetSpellBar(null);
    }

    public void SetSpellBar(PlayerUnit unit)
    {
        for (int i = 0; i < spellIcons.Length; i++)
        {
            if (unit == null || unit.spells.Count <= i) spellIcons[i].ClearSlot();
            else if (unit != null) SetSpell(spellIcons[i], unit.spells[i]);
        }
    }

    public void SetSpell(SpellIconUI spellUI, Spell spell)
    {
        spellUI.SetSlot(spell);
    }
}
