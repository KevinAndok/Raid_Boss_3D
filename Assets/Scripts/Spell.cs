using System;
using UnityEngine;

[Serializable]
public sealed class Spell
{
    public string spellName;
    public string spellDescription;
    public Sprite spellIcon;
    public int spellID;
    public float cooldown;
    public float lastCastTime = 0;

    public ICommand GetCommand(Entity self)
    {
        switch (spellID)
        {
            case 0:
                return new TestingSpell(self, this, PlayerCommander.mouseGroundPoint);
            //case 1:
            //    return new TestingSpell(self, PlayerCommander.mouseGroundPoint);
            default:
                Debug.LogError("spellID not implemented");
                return null;
        }
    }
}
