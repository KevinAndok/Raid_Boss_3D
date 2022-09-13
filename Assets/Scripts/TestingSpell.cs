using UnityEngine;

public sealed class TestingSpell : ICommand //ID 0
{
    Vector3 Ground;
    float time = 2;
    Spell Spell;

    public TestingSpell(Entity self, Spell spell, Vector3 ground)
    {
        Self = self;
        Ground = ground;
        Spell = spell;

        if (self.commands.Count > 0 && self.commands[0].GetType() == typeof(WaitCommand)) self.commands.RemoveAt(0);
        if (self.commands.Count == 0) BeginExecute();
    }

    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

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
        Debug.Log(time);
        if (time <= 0)
        {
            Self.transform.position = Ground;
            Self.navigation.destination = Ground;
            OnComplete();
        }
    }

    public void OnInterrupt()
    {
        OnComplete();
    }
}
