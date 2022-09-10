using System;
using UnityEditor;
using UnityEngine;

[Serializable]
public class Spell
{
    public string spellName;
    public string spellDescription;
    public Sprite spellIcon;
    public MonoScript command;
}
