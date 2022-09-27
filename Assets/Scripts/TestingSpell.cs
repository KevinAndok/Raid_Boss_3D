using UnityEngine;
using UnityEngine.UIElements;

public sealed class TestingSpell : ICommand //ID 0
{
    Vector3 position;
    float time = 2;
    Spell Spell;

    public TestingSpell(Entity self, Spell spell, Vector3 ground)
    {
        Self = self;
        position = ground;
        Spell = spell;

        if (self.commands.Count > 0 && self.commands[0].GetType() == typeof(WaitCommand)) self.commands.RemoveAt(0);
        if (self.commands.Count == 0) BeginExecute();
    }

    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    public OrderType Type => OrderType.utility;
    public GameObject WaypointObject { get; set; }

    public void BeginExecute()
    {
        if (!Self.CanCastSpells)
        {
            OnComplete();
            return;
        }

        Self.navigation.destination = Self.transform.position;
        Spell.lastCastTime = Time.time;
        BeingExecuted = true;
        Self.CastAnimation(true);
    }

    public void DisplayCommand(Pool waypointPool)
    {
        var waypoint = waypointPool.ObjectPool.Get();
        waypoint.GetComponent<Waypoint>().Set(position, Self.entitySize, Type);
        WaypointObject = waypoint;
    }

    public void OnCancel()
    {
        Self.CastAnimation(false);
        Self.NextCommand();
    }

    public void OnComplete()
    {
        Self.CastAnimation(false);
        Self.NextCommand();
    }

    public void OnFixedFrame()
    {
        if (!Self.CanCastSpells)
        {
            OnComplete();
            return;
        }

        time -= Time.fixedDeltaTime;

        if (time <= 0)
        {
            Self.transform.position = position;
            Self.navigation.destination = position;
            OnComplete();
        }
    }

    public void OnInterrupt()
    {
        OnComplete();
    }
}
