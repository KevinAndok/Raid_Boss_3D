using UnityEngine;
using System;

[Serializable]
public sealed class Debuffs
{
    const float BLEEDMODIFIER = -25f;
    const float POISONMODIFIER = 25f;

    private float bleedingDamage, bleedingTime; //decreases over time
    private float poisonDamage, poisonTime;     //increases over time
    private float burningDamage, burningTime;   //same damage over time

    private float stunTime;
    private float freezeTime;
    private float terrorizeTime;
    private float silenceTime;

    //todo: other stat debuffs

    public bool IsStunned { get => stunTime > Time.time; }
    public bool IsFrozen { get => freezeTime > Time.time; }
    public bool IsTerrorized { get => terrorizeTime > Time.time; }
    public bool IsSilenced { get => silenceTime > Time.time; }

    public bool IsBurning { get => burningTime > Time.time; }
    public bool IsPoisoned { get => poisonTime > Time.time; }
    public bool IsBleeding { get => bleedingTime > Time.time; }

    private Entity self;

    public void Init(Entity self)
    {
        this.self = self;

        bleedingDamage = 0;
        bleedingTime = 0;
        poisonDamage = 0;
        poisonTime = 0;
        burningDamage = 0;
        burningTime = 0;
        stunTime = 0;
        freezeTime = 0;
        terrorizeTime = 0;
        silenceTime = 0;
    }
    public void FixedUpdateJob()
    {
        var damage = (IsBleeding ? bleedingDamage : 0) + (IsPoisoned ? poisonDamage : 0) + (IsBurning ? burningDamage : 0);
        self.stats.Damage(damage);

        poisonDamage += (poisonDamage * POISONMODIFIER) * Time.fixedDeltaTime;
        bleedingDamage += (bleedingDamage * BLEEDMODIFIER) * Time.fixedDeltaTime;
    }

    public void ApplySlow()
    {
        //todo
    }
    public void InterruptCasting()
    {
        self.commands[0].OnInterrupt();
    }
    public void ApplyBleed(float dps)
    {
        if (!IsBleeding) bleedingDamage = dps;
        else bleedingDamage += dps;
        bleedingTime = Time.time + 6;
    }
    public void ApplyPoison(float dps)
    {
        if (!IsPoisoned) poisonDamage = dps;
        else poisonDamage += dps;
        poisonTime = Time.time + 2;
    }
    public void ApplyBurn(float dps)
    {
        if (!IsBurning) burningDamage = dps;
        else burningDamage += dps;
        burningTime = Time.time + 4;
    }
    public void Stun(float duration)
    {
        stunTime = Time.time + duration;
        InterruptCasting();
        self.StopAllCommands();
        //TODO: Stun effect
    }
    public void Freeze(float duration)
    {
        freezeTime = Time.time + duration;
        InterruptCasting();
        self.StopAllCommands();
        //TODO: Freeze effect
    }
    public void Silence(float duration)
    {
        freezeTime = Time.time + duration;
        InterruptCasting();
        //TODO: Silence effect
    }
    public void Terrorize(float duration, Vector3 position)
    {
        terrorizeTime = Time.time + duration;
        position = self.transform.position - position;
        self.StopAllCommands();
        self.commands.Add(new MoveCommand(self, position));
    }
}
