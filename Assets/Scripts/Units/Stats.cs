using System;
using UnityEngine;

[Serializable]
public sealed class Stats
{
    #region ScaleConstants
    private const float MAX_ATTACK_SPEED_VALUE = 100;
    private const float MAX_RESISTANCE_VALUE = 100;
    private const float CRIT_DAMAGE_MULTIPLIER = 2;

    private const float STRENGTH_DAMAGE_SCALE = 1;
    private const float STRENGTH_PHYSICAL_RESISTANCE_SCALE = 1;

    private const float AGILITY_ATTACKSPEED_SCALE = 1;
    private const float AGILITY_MOVEMENT_SCALE = 1;

    private const float VITALITY_HEALTH_SCALE = 1;
    private const float VITALITY_HEALTHREGEN_SCALE = 1;

    private const float DEXERITY_CRITICAL_CHANCE_SCALE = .01f;
    private const float DEXERITY_HITCHANCE_SCALE = 1;

    private const float INTELLIGENCE_MANA_SCALE = 1;
    private const float INTELLIGENCE_MANAREGEN_SCALE = 1;

    private const float WISDOM_SPELLDAMAGE_SCALE = 1;
    private const float WISDOM_MAGIC_RESISTANCE__SCALE = 1;
    #endregion

    #region Editor Variables

    [SerializeField] private float baseMovementSpeed;
    [Space(10)]
    [SerializeField] private float baseHealth;
    [SerializeField] private float baseHealthRegeneration;
    [SerializeField] private float baseMana;
    [SerializeField] private float baseManaRegeneration;
    [Space(10)]
    [SerializeField] private float baseAttackSpeed;
    [SerializeField] private float baseAttackRange;
    [SerializeField] private float baseAttackDamage;
    [SerializeField][Range(0, 1)] private float baseHitChance;
    [SerializeField][Range(0, 1)] private float baseCritChance;
    [SerializeField][Min(0)] private float baseSpellDamage;
    [Space(10)]
    [SerializeField] private float baseStrength;        //physical damage, physical resistance
    [SerializeField] private float baseAgility;         //attack speed, movement speed
    [SerializeField] private float baseVitality;        //health, health regeneration
    [SerializeField] private float baseDexerity;        //crit chance, hit chance
    [SerializeField] private float baseIntelligence;    //mana, mana regeneration
    [SerializeField] private float baseWisdom;          //spell damage, magical resistance 
    [Space(10)]
    [SerializeField] private float basePhysicalArmor;
    [SerializeField] private float baseMagicArmor;
    [SerializeField] private float baseBleedResistance;
    [SerializeField] private float basePoisonResistance;
    [SerializeField] private float baseBurnResistance;
    [Space(10)]
    public bool MovementSlowImmune;
    public bool AttackSlowImmune;
    public bool StunImmune;
    public bool FreezeImmune;
    public bool TerrorizeImmune;
    public bool SilenceImmune;

    #endregion

    #region Properties
    public float Health { get; private set; }
    public float MaxHealth
    {
        get => bonusHealth + baseHealth;
        private set
        {
            float healthPercentage = Health / MaxHealth;
            bonusHealth = value;
            Health = MaxHealth * healthPercentage;
        }
    }
    public float HealthRegeneration { get => bonusHealthRegeneration + baseHealthRegeneration; private set { bonusHealthRegeneration = value; } }
    public float Mana { get; private set; }
    public float MaxMana
    {
        get => bonusMana + baseMana;
        private set
        {
            float healthPercentage = Mana / MaxMana;
            bonusMana = value;
            Mana = MaxMana * healthPercentage;
        }
    }
    public float ManaRegeneration { get => bonusManaRegeneration + baseManaRegeneration; private set { bonusManaRegeneration = value; } }
    public float MovementSpeed { get => bonusMovementSpeed + baseMovementSpeed; private set { bonusMovementSpeed = value; } }
    public float AttackSpeed { get => (bonusAttackSpeed + baseAttackSpeed) / MAX_ATTACK_SPEED_VALUE; private set { bonusAttackSpeed = Mathf.Clamp(value, 0, MAX_ATTACK_SPEED_VALUE - baseAttackSpeed); } }
    public float AttackRange { get => baseAttackRange; }
    public float AttackDamage { get => bonusAttackDamage + baseAttackDamage; private set { bonusAttackDamage = value; } }
    public float HitChance { get => bonusHitChance + baseHitChance; private set { bonusHitChance = Mathf.Clamp(value, 0, 1 - baseHitChance); } }
    public float CritChance { get => bonusCritChance + baseCritChance; private set { bonusCritChance = Mathf.Clamp(value, 0, 1 - baseCritChance); } }
    public float SpellDamage { get => bonusSpellDamage + baseSpellDamage; private set { bonusSpellDamage = value; } }

    public float Strength { get => bonusStrength + baseStrength; }
    public float Agility { get => bonusAgility + baseAgility; }
    public float Vitality { get => bonusVitality + baseVitality; }
    public float Dexerity { get => bonusDexerity + baseDexerity; }
    public float Intelligence { get => bonusIntelligence + baseIntelligence; }
    public float Wisdom { get => bonusWisdom + baseWisdom; }

    public float LastAttack { get; private set; }

    #region Resistances

    #region Physical
    public float PhysicalResistance
    {
        get => bonusPhysicalResistance + basePhysicalArmor;
        set => bonusPhysicalResistance = Mathf.Clamp(value, 0, MAX_RESISTANCE_VALUE - basePhysicalArmor);
    }
    public float PhysicalResistancePercentage { get => PhysicalResistance / MAX_RESISTANCE_VALUE; }
    #endregion
    #region Magic
    public float MagicResistance
    {
        get => bonusMagicResistance + baseMagicArmor;
        set => bonusMagicResistance = Mathf.Clamp(value, 0, MAX_RESISTANCE_VALUE - baseMagicArmor);
    }
    public float MagicResistancePercentage { get => MagicResistance / MAX_RESISTANCE_VALUE; }
    #endregion
    #region Bleeding
    public float BleedResistance
    {
        get => bonusBleedResistance + baseBleedResistance;
        set => bonusBleedResistance = Mathf.Clamp(value, 0, MAX_RESISTANCE_VALUE - baseBleedResistance);
    }
    public float BleedResistancePercentage { get => BleedResistance / MAX_RESISTANCE_VALUE; }
    #endregion
    #region Poison
    public float PoisonResistance
    {
        get => bonusPoisonResistance + basePoisonResistance;
        set => bonusPoisonResistance = Mathf.Clamp(value, 0, MAX_RESISTANCE_VALUE - basePoisonResistance);
    }
    public float PoisonResistancePercentage { get => PoisonResistance / MAX_RESISTANCE_VALUE; }
    #endregion
    #region Burning
    public float BurnResistance
    {
        get => bonusBurnResistance + baseBurnResistance;
        set => bonusBurnResistance = Mathf.Clamp(value, 0, MAX_RESISTANCE_VALUE - baseBurnResistance);
    }
    public float BurnResistancePercentage { get => BurnResistance / MAX_RESISTANCE_VALUE; }
    #endregion

    #endregion

    #endregion

    #region Private Variables

    private float bonusHealth;
    private float bonusHealthRegeneration;

    private float bonusMana;
    private float bonusManaRegeneration;

    private float bonusAttackDamage;
    private float bonusAttackSpeed;
    private float bonusHitChance;
    private float bonusCritChance;
    private float bonusSpellDamage;
    private float bonusMovementSpeed;

    private float bonusBurnResistance;
    private float bonusPoisonResistance;
    private float bonusBleedResistance;
    private float bonusMagicResistance;
    private float bonusPhysicalResistance;

    private float bonusStrength;
    private float bonusAgility;
    private float bonusVitality;
    private float bonusDexerity;
    private float bonusIntelligence;
    private float bonusWisdom;

    private Entity Self;

    #endregion

    public event Action OnDeath;

    public void Init(Entity self)
    {
        this.Self = self;

        StrengthUpdate();
        AgilityUpdate();
        VitalityUpdate();
        DexerityUpdate();
        IntelligenceUpdate();
        WisdomUpdate();

        MovementSpeedUpdate();

        Health = MaxHealth;
        Mana = MaxMana;

        LastAttack = 0;
    }

    #region StatUpdateMethods

    //TODO: add bonus from items

    #region Attack Speed
    void AttackSpeedUpdate()
    {
        //TODO: set animation speed
    }
    public void AddAttackSpeed(float percentage)
    {
        bonusMovementSpeed += AttackSpeed * percentage;
        AttackSpeedUpdate();
    }
    #endregion
    #region Movement Speed
    void MovementSpeedUpdate()
    {
        Self.navigation.speed = MovementSpeed;
    }
    public void AddMovementSpeed(float percentage)
    {
        bonusMovementSpeed += MovementSpeed * percentage;
        MovementSpeedUpdate();
    }
    #endregion
    #region Strength
    void StrengthUpdate()
    {
        AttackDamage = Strength * STRENGTH_DAMAGE_SCALE;
        PhysicalResistance = Strength * STRENGTH_PHYSICAL_RESISTANCE_SCALE;
    }
    public void AddStrength(float value)
    {
        bonusStrength += value;
        StrengthUpdate();
    }
    #endregion
    #region Agility
    void AgilityUpdate()
    {
        AttackSpeed = Agility * AGILITY_ATTACKSPEED_SCALE;
        MovementSpeed = Agility * AGILITY_MOVEMENT_SCALE;
    }
    public void AddAgility(float value)
    {
        bonusAgility += value;
        AgilityUpdate();
    }
    #endregion
    #region Vitality
    void VitalityUpdate()
    {
        MaxHealth = Vitality * VITALITY_HEALTH_SCALE;
        HealthRegeneration = Vitality * VITALITY_HEALTHREGEN_SCALE;
    }
    public void AddVitality(float value)
    {
        bonusVitality += value;
        VitalityUpdate();
    }
    #endregion
    #region Dexerity
    void DexerityUpdate()
    {
        CritChance = Dexerity * DEXERITY_CRITICAL_CHANCE_SCALE;
        HitChance = Dexerity * DEXERITY_HITCHANCE_SCALE;
    }
    public void AddDexerity(float value)
    {
        bonusDexerity += value;
        DexerityUpdate();
    }
    #endregion
    #region Intelligence
    void IntelligenceUpdate()
    {
        MaxMana = Intelligence * INTELLIGENCE_MANA_SCALE;
        ManaRegeneration = Intelligence * INTELLIGENCE_MANAREGEN_SCALE;
    }
    public void AddIntelligence(float value)
    {
        bonusIntelligence += value;
        IntelligenceUpdate();
    }
    #endregion
    #region Wisdom
    void WisdomUpdate()
    {
        SpellDamage = baseSpellDamage + Wisdom * WISDOM_SPELLDAMAGE_SCALE;
        MagicResistance = Wisdom * WISDOM_MAGIC_RESISTANCE__SCALE;
    }
    public void AddWisdom(float value)
    {
        bonusWisdom += value;
        WisdomUpdate();
    }
    #endregion

    #endregion

    public void Attack(Entity target)
    {
        Self.AttackAnimation();
        LastAttack = Time.time;
        if (UnityEngine.Random.value > HitChance) return;
        target.stats.Damage(UnityEngine.Random.value <= CritChance ? AttackDamage * CRIT_DAMAGE_MULTIPLIER : AttackDamage);
    }
    public void Damage(float amount)
    {
        amount *= 1 - PhysicalResistancePercentage;
        if (amount == 0) return;
        Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
        if (Health == 0) OnDeath?.Invoke();
    }
    public void Heal(float amount) => Health = Mathf.Clamp(baseHealth + amount, 0, MaxHealth);
    public void RegenerateHealth()
    {
        if (Self.IsDead)
            Heal(HealthRegeneration * Time.fixedDeltaTime);
    }

    public void DisplayStats()
    {
        string statsMessage =
            $"Health: {Health} \n" +
            $"MaxHealth: {MaxHealth} \n" +
            $"HealthRegeneration: {HealthRegeneration} \n" +
            $"Mana: {Mana} \n" +
            $"MaxMana: {MaxMana} \n" +
            $"ManaRegeneration: {ManaRegeneration} \n" +
            $"MovementSpeed: {MovementSpeed} \n" +
            $"AttackSpeed: {AttackSpeed} \n" +
            $"AttackRange: {AttackRange} \n" +
            $"AttackDamage: {AttackDamage} \n" +
            $"HitChance: {HitChance} \n" +
            $"CritChance: {CritChance} \n" +
            $"SpellDamage: {SpellDamage} \n" +
            $"Strength: {Strength} \n" +
            $"Agility: {Agility} \n" +
            $"Vitality: {Vitality} \n" +
            $"Dexerity: {Dexerity} \n" +
            $"Intelligence: {Intelligence} \n" +
            $"Wisdom: {Wisdom} \n" +
            $"PhysicalResistance: {PhysicalResistancePercentage} \n" +
            $"BleedResistance: {BleedResistance} \n" +
            $"PoisonResistance: {PoisonResistance} \n" +
            $"BurnResistance: {BurnResistance}";

        Debug.Log(statsMessage);
    }
}
