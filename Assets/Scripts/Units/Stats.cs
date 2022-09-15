using System;
using UnityEngine;

[Serializable]
public sealed class Stats
{
    #region ScaleConstants
    private const float STRENGTH_DAMAGE_SCALE = 1;
    private const float AGILITY_ATTACKSPEED_SCALE = 1;
    private const float VITALITY_HEALTH_SCALE = 1;
    private const float VITALITY_HEALTHREGEN_SCALE = 1;
    private const float DEXERITY_MOVEMENT_SCALE = 1;
    private const float DEXERITY_HITCHANCE_SCALE = 1;
    private const float INTELLIGENCE_MANA_SCALE = 1;
    private const float INTELLIGENCE_MANAREGEN_SCALE = 1;
    private const float WISDOM_SPELLDAMAGE_SCALE = 1;
    #endregion

    [SerializeField] private float baseHealth;
    [SerializeField] private float baseMana;
    [SerializeField] private float baseMovementSpeed;
    [SerializeField] private float baseAttackSpeed;
    [SerializeField] private float baseHealthRegeneration;
    [SerializeField] private float baseManaRegeneration;
    [SerializeField] private float baseAttackRange;
    [SerializeField] private float baseAttackDamage;
    [SerializeField][Range(0, 1)] private float baseHitChance;
    [SerializeField][Range(0, 1)] private float baseCritChance;
    [SerializeField][Min(0)] private float baseSpellDamage;
    [Space(10)]
    [SerializeField] private float baseStrength;        //damage
    [SerializeField] private float baseAgility;         //attack speed
    [SerializeField] private float baseVitality;        //health, health regeneration
    [SerializeField] private float baseDexerity;        //movement speed, hit chance
    [SerializeField] private float baseIntelligence;    //mana, mana regeneration
    [SerializeField] private float baseWisdom;          //spell damage

    #region Properties
    public float Health { get; private set; }
    public float Mana { get; private set; }
    public float MaxHealth { get; private set; }
    public float MaxMana { get; private set; }
    public float HealthRegeneration { get; private set; }
    public float ManaRegeneration { get; private set; }
    public float MovementSpeed { get; private set; }
    public float BaseMovementSpeed { get; private set; }
    public float AttackSpeed { get; private set; }
    public float AttackRange { get; private set; }
    public float AttackDamage { get; private set; }
    public float HitChance { get; private set; }
    public float CritChance { get; private set; }
    public float SpellDamage { get; private set; }

    public float Strength { get; private set; }
    public float Agility { get; private set; }
    public float Vitality { get; private set; }
    public float Dexerity { get; private set; }
    public float Intelligence { get; private set; }
    public float Wisdom { get; private set; }

    public float LastAttack { get; private set; }
    #endregion

    Entity self;

    public event Action OnDeath;

    public void Init(Entity self)
    {
        this.self = self;

        StrengthUpdate(0);
        AgilityUpdate(0);
        VitalityUpdate(0);
        DexerityUpdate(0);
        IntelligenceUpdate(0);
        WisdomUpdate(0);

        Health = MaxHealth;
        Mana = MaxMana;
        MovementSpeed = baseMovementSpeed;
        AttackRange = baseAttackRange;

        LastAttack = 0;

        self.navigation.speed = baseMovementSpeed;
    }

    #region StatUpdateMethods
    void StrengthUpdate(float value)
    {
        Strength = baseStrength + value;
        AttackDamage = baseAttackDamage + Strength * STRENGTH_DAMAGE_SCALE;
    }
    void AgilityUpdate(float value)
    {
        Agility = baseAgility + value;
        AttackSpeed = baseAttackSpeed + Agility * AGILITY_ATTACKSPEED_SCALE;
    }
    void VitalityUpdate(float value)
    {
        Vitality = baseVitality + value;
        MaxHealth = baseHealth + Vitality * VITALITY_HEALTH_SCALE;
        HealthRegeneration = baseHealthRegeneration + Vitality * VITALITY_HEALTHREGEN_SCALE;
    }
    void DexerityUpdate(float value)
    {
        Dexerity = baseVitality + value;
        BaseMovementSpeed = baseMovementSpeed + Dexerity * DEXERITY_MOVEMENT_SCALE;
        HitChance = Mathf.Clamp01(baseHealthRegeneration + Dexerity * DEXERITY_HITCHANCE_SCALE);
    }
    void IntelligenceUpdate(float value)
    {
        Intelligence = baseVitality + value;
        MaxMana = baseMana + Intelligence * INTELLIGENCE_MANA_SCALE;
        ManaRegeneration = baseManaRegeneration + Intelligence * INTELLIGENCE_MANAREGEN_SCALE;
    }
    void WisdomUpdate(float value)
    {
        Wisdom = baseVitality + value;
        SpellDamage = baseSpellDamage + Wisdom * WISDOM_SPELLDAMAGE_SCALE;
    }
    #endregion

    public void Attack(Entity target)
    {
        self.AttackAnimation();
        LastAttack = Time.time;
        target.stats.Damage(AttackDamage);
    }
    public void Damage(float amount)
    {
        if (amount == 0) return;
        Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
        if (Health == 0) OnDeath?.Invoke();
    }
    public void Heal(float amount) => Health = Mathf.Clamp(baseHealth + amount, 0, MaxHealth);
    public void RegenerateHealth()
    {
        if (self.IsDead)
            Heal(HealthRegeneration * Time.fixedDeltaTime);
    }
}
