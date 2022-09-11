using System;
using UnityEngine;

[Serializable]
public class Spell
{
    public string spellName;
    public string spellDescription;
    public Sprite spellIcon;
    public int spellID;

    public ICommand GetCommand(Entity self)
    {
        switch (spellID)
        {
            case 0:
                return new TestingSpell(self, PlayerCommander.pointingAtGround);
            default:
                return null;
        }
    }
}
