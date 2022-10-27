using UnityEngine;

public sealed class AOETerrorize : ICommand //ID 0
{
    const float DURATION = 3f;
    const float DISTANCE = 3f;

    Spell Spell;

    public AOETerrorize(Entity self, Spell spell)
    {
        Self = self;
        Spell = spell;

        if (self.commands.Count > 0 && self.commands[0].GetType() == typeof(WaitCommand)) self.commands.RemoveAt(0);
        if (self.commands.Count == 0) BeginExecute();
    }

    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    public OrderType Type => OrderType.utility;
    public GameObject WaypointObject { get; set; }

    public float? castTime => null;

    public void DisplayCommand(Pool waypointPool)
    {
        //var waypoint = waypointPool.ObjectPool.Get();
        //waypoint.GetComponent<Waypoint>().Set(Self.transform, Self.entitySize, Type);
        //WaypointObject = waypoint;
    }

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

        Self.PowerUpAnimation();
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
        foreach (var entity in Physics.SphereCastAll(Self.transform.position, DISTANCE, Vector3.up, .1f, LayerMask.GetMask("Entity")))
        {
            if (entity.collider.TryGetComponent(out Entity ent) && ent.team == Team.boss)
            {
                ent.debuffs.Terrorize(DURATION, Self.transform.position);
                Debug.Log(ent);
            }
        }

        OnComplete();
    }

    public void OnInterrupt()
    {
        OnComplete();
    }
}
