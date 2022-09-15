using System;
using System.Collections;
using UnityEngine;

[Serializable]
public sealed class Debuffs
{
    const float BLEED_MODIFIER = -25f;
    const float POISON_MODIFIER = 25f;

    private float bleedingDamage, bleedingTime; //decreases over time
    private float poisonDamage, poisonTime;     //increases over time
    private float burningDamage, burningTime;   //same damage over time

    private float stunTime;
    private float freezeTime;
    private float terrorizeTime;
    private float silenceTime;

    //strength
    private float strengthDebuffTime;
    private float strengthDebuffValue;
    //agility
    private float agilityDebuffTime;
    private float agilityDebuffValue;
    //vitality
    private float vitalityDebuffTime;
    private float vitalityDebuffValue;
    //dexerity
    private float dexerityDebuffTime;
    private float dexerityDebuffValue;
    //intelligence
    private float intelligenceDebuffTime;
    private float intelligenceDebuffValue;
    //wisdom
    private float wisdomDebuffTime;
    private float wisdomDebuffValue;

    public bool IsStunned { get => stunTime > Time.time; }
    public bool IsFrozen { get => freezeTime > Time.time; }
    public bool IsTerrorized { get => terrorizeTime > Time.time; }
    public bool IsSilenced { get => silenceTime > Time.time; }

    public bool IsBurning { get => burningTime > Time.time; }
    public bool IsPoisoned { get => poisonTime > Time.time; }
    public bool IsBleeding { get => bleedingTime > Time.time; }

    public bool StrengthDebuff { get => strengthDebuffTime > Time.time; }
    public bool AgilityDebuff { get => agilityDebuffTime > Time.time; }
    public bool VitalityDebuff { get => vitalityDebuffTime > Time.time; }
    public bool DexerityDebuff { get => dexerityDebuffTime > Time.time; }
    public bool IntelligenceDebuff { get => intelligenceDebuffTime > Time.time; }
    public bool WisdomDebuff { get => wisdomDebuffTime > Time.time; }

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

    #region Fixed Update Methods
    public void ApplyDotDamage()
    {
        var damage = 
            (IsBleeding ? bleedingDamage * (1 - self.stats.BleedResistance) : 0) + 
            (IsPoisoned ? poisonDamage * (1 - self.stats.PoisonResistance) : 0) + 
            (IsBurning ? burningDamage * (1 - self.stats.BurnResistance) : 0);
        self.stats.Damage(damage);

        poisonDamage += (poisonDamage * POISON_MODIFIER) * Time.fixedDeltaTime;
        bleedingDamage += (bleedingDamage * BLEED_MODIFIER) * Time.fixedDeltaTime;
    }
    public void StatDebuffExpiration()
    {
        if (!StrengthDebuff && strengthDebuffValue != 0)
        {
            self.stats.AddStrength(strengthDebuffValue);
            strengthDebuffValue = 0;
        }
        if (!AgilityDebuff && agilityDebuffValue != 0)
        {
            self.stats.AddAgility(agilityDebuffValue);
            agilityDebuffValue = 0;
        }
        if (!VitalityDebuff && vitalityDebuffValue != 0)
        {
            self.stats.AddVitality(vitalityDebuffValue);
            vitalityDebuffValue = 0;
        }
        if (!DexerityDebuff && dexerityDebuffValue != 0)
        {
            self.stats.AddDexerity(dexerityDebuffValue);
            dexerityDebuffValue = 0;
        }
        if (!IntelligenceDebuff && intelligenceDebuffValue != 0)
        {
            self.stats.AddIntelligence(intelligenceDebuffValue);
            intelligenceDebuffValue = 0;
        }
        if (!WisdomDebuff && wisdomDebuffValue != 0)
        {
            self.stats.AddWisdom(wisdomDebuffValue);
            wisdomDebuffValue = 0;
        }
    }
    #endregion

    #region Stat Debuffs

    #region Strength
    public void DebuffStrength(float value, float duration)
    {
        if (StrengthDebuff && strengthDebuffValue > value) return;
        self.stats.AddStrength(-strengthDebuffValue);
        strengthDebuffValue = value;
        strengthDebuffTime = Time.time + duration;
    }
    #endregion
    #region Agility
    public void DebuffAgility(float value, float duration)
    {
        if (AgilityDebuff && agilityDebuffValue > value) return;
        self.stats.AddAgility(-agilityDebuffValue);
        agilityDebuffValue = value;
        agilityDebuffTime = Time.time + duration;
    }
    #endregion
    #region Vitality
    public void DebuffVitality(float value, float duration)
    {
        if (VitalityDebuff && vitalityDebuffValue > value) return;
        self.stats.AddVitality(-vitalityDebuffValue);
        vitalityDebuffValue = value;
        vitalityDebuffTime = Time.time + duration;
    }
    #endregion
    #region Dexerity
    public void DebuffDexerity(float value, float duration)
    {
        if (DexerityDebuff && dexerityDebuffValue > value) return;
        self.stats.AddDexerity(-dexerityDebuffValue);
        dexerityDebuffValue = value;
        dexerityDebuffTime = Time.time + duration;
    }
    #endregion
    #region Intelligence
    public void DebuffIntelligence(float value, float duration)
    {
        if (IntelligenceDebuff && intelligenceDebuffValue > value) return;
        self.stats.AddIntelligence(-intelligenceDebuffValue);
        intelligenceDebuffValue = value;
        intelligenceDebuffTime = Time.time + duration;
    }
    #endregion
    #region Wisdom
    public void DebuffWisdom(float value, float duration)
    {
        if (WisdomDebuff && wisdomDebuffValue > value) return;
        self.stats.AddWisdom(-wisdomDebuffValue);
        wisdomDebuffValue = value;
        wisdomDebuffTime = Time.time + duration;
    }
    #endregion

    #endregion
    #region Slow
    public void ApplySlow(float duration, float percentage)
    {
        if (self.stats.SlowImmune) return;
        self.StartCoroutine(SlowDebuff(duration, percentage));
    }
    IEnumerator SlowDebuff(float duration, float percentage)
    {
        var stats = self.stats;
        var value = stats.MovementSpeed * percentage;
        float startTime = Time.time;

        stats.MovementSpeedUpdate(stats.MovementSpeed - value);

        yield return new WaitForSeconds(duration);

        while (Time.time < startTime + duration)
        {
            //if unit becomes slow immune while slowed, we want to stop the slow debuff
            if (stats.SlowImmune) break;
            yield return null;
        }

        stats.MovementSpeedUpdate(stats.MovementSpeed + value);
    }
    #endregion
    #region Interrupt
    public void InterruptCasting()
    {
        self.commands[0].OnInterrupt();
    }
    #endregion
    #region Bleed
    public void ApplyBleed(float dps)
    {
        if (!IsBleeding) bleedingDamage = dps;
        else bleedingDamage += dps;
        bleedingTime = Time.time + 6;
    }
    #endregion
    #region Poison
    public void ApplyPoison(float dps)
    {
        if (!IsPoisoned) poisonDamage = dps;
        else poisonDamage += dps;
        poisonTime = Time.time + 2;
    }
    #endregion
    #region Burn
    public void ApplyBurn(float dps)
    {
        if (!IsBurning) burningDamage = dps;
        else burningDamage += dps;
        burningTime = Time.time + 4;
    }
    #endregion
    #region Stun
    public void Stun(float duration)
    {
        if (self.stats.StunImmune) return;
        stunTime = Time.time + duration;
        InterruptCasting();
        self.StopAllCommands();
        //TODO: Stun effect
    }
    #endregion
    #region Freeze
    public void Freeze(float duration)
    {
        if (self.stats.FreezeImmune) return;
        freezeTime = Time.time + duration;
        InterruptCasting();
        self.StopAllCommands();
        //TODO: Freeze effect
    }
    #endregion
    #region Silence
    public void Silence(float duration)
    {
        if (self.stats.SilenceImmune) return;
        freezeTime = Time.time + duration;
        InterruptCasting();
        //TODO: Silence effect
    }
    #endregion
    #region Terrorize
    public void Terrorize(float duration, Vector3 position)
    {
        if (self.stats.TerrorizeImmune) return;
        terrorizeTime = Time.time + duration;
        position = self.transform.position - position;
        self.StopAllCommands();
        self.commands.Add(new MoveCommand(self, position));
    }
    #endregion
}
