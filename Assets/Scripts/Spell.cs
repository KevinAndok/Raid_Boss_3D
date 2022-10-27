using UnityEngine;

[CreateAssetMenu(fileName = "New Spell", menuName = "ScriptableObjects/Spell", order = 1)]
public sealed class Spell : ScriptableObject
{
    //Spells are duplicated in Start for every (player unit) entity so they dont get the same cooldowns
    //Keep this in mind when creating new variables for spells
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
            case 1:
                return new AOETerrorize(self, this);
            default:
                Debug.LogError("spellID not implemented");
                return null;
        }
    }
}
