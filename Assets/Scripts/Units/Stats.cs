using System;
using UnityEngine;

[Serializable]
public sealed class Stats
{
    #region ScaleConstants
    private const float CRIT_DAMAGE_MULTIPLIER = 2;
    private const float MAX_RESISTANCE_VALUE = 100;
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
    [SerializeField] private float baseStrength;        //damage
    [SerializeField] private float baseAgility;         //attack speed
    [SerializeField] private float baseVitality;        //health, health regeneration
    [SerializeField] private float baseDexerity;        //movement speed, hit chance
    [SerializeField] private float baseIntelligence;    //mana, mana regeneration
    [SerializeField] private float baseWisdom;          //spell damage
    [Space(10)]
    [SerializeField] private float physicalArmor;
    [SerializeField] private float magicArmor;
    [SerializeField] private float bleedResistance;
    [SerializeField] private float poisonResistance;
    [SerializeField] private float burnResistance;
    [Space(10)]
    public bool SlowImmune;
    public bool StunImmune;
    public bool FreezeImmune;
    public bool TerrorizeImmune;
    public bool SilenceImmune;

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

    #region Resistances
    public float PhysicalResistance
    {
        get => physicalArmor / MAX_RESISTANCE_VALUE;
        private set { physicalArmor = Mathf.Clamp(value, 0, MAX_RESISTANCE_VALUE); }
    }
    public float MagicResistance
    {
        get => magicArmor / MAX_RESISTANCE_VALUE;
        private set { magicArmor = Mathf.Clamp(value, 0, MAX_RESISTANCE_VALUE); }
    }
    public float BleedResistance
    {
        get => bleedResistance / MAX_RESISTANCE_VALUE;
        private set { bleedResistance = Mathf.Clamp(value, 0, MAX_RESISTANCE_VALUE); }
    }
    public float PoisonResistance
    {
        get => poisonResistance / MAX_RESISTANCE_VALUE;
        private set { poisonResistance = Mathf.Clamp(value, 0, MAX_RESISTANCE_VALUE); }
    }
    public float BurnResistance
    {
        get => burnResistance / MAX_RESISTANCE_VALUE;
        private set { burnResistance = Mathf.Clamp(value, 0, MAX_RESISTANCE_VALUE); }
    }
    #endregion

    #endregion

    Entity self;

    public event Action OnDeath;

    public void Init(Entity self)
    {
        this.self = self;

        //zeros = stats from items
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

    #region Movement Speed
    public void MovementSpeedUpdate(float value)
    {
        MovementSpeed = value;
    }
    #endregion
    #region Strength
    void StrengthUpdate(float value, bool resetValue = true)
    {
        Strength = resetValue ? baseStrength + value : value;
        AttackDamage = baseAttackDamage + Strength * STRENGTH_DAMAGE_SCALE;
    }
    public void AddStrength(float value)
    {
        Strength += value;
        StrengthUpdate(Strength, true);
    }
    #endregion
    #region Agility
    void AgilityUpdate(float value, bool resetValue = true)
    {
        Agility = resetValue ? baseAgility + value : value;
        AttackSpeed = baseAttackSpeed + Agility * AGILITY_ATTACKSPEED_SCALE;
    }
    public void AddAgility(float value)
    {
        Agility += value;
        AgilityUpdate(Agility, false);
    }
    #endregion
    #region Vitality
    void VitalityUpdate(float value, bool resetValue = true)
    {
        Vitality = resetValue ? baseVitality + value : value;
        MaxHealth = baseHealth + Vitality * VITALITY_HEALTH_SCALE;
        HealthRegeneration = baseHealthRegeneration + Vitality * VITALITY_HEALTHREGEN_SCALE;
    }
    public void AddVitality(float value)
    {
        Vitality += value;
        VitalityUpdate(Vitality, false);
    }
    #endregion
    #region Dexerity
    void DexerityUpdate(float value, bool resetValue = true)
    {
        Dexerity = resetValue ? baseDexerity + value : value;
        BaseMovementSpeed = baseMovementSpeed + Dexerity * DEXERITY_MOVEMENT_SCALE;
        HitChance = Mathf.Clamp01(baseHealthRegeneration + Dexerity * DEXERITY_HITCHANCE_SCALE);
    }
    public void AddDexerity(float value)
    {
        Dexerity += value;
        DexerityUpdate(Dexerity, false);
    }
    #endregion
    #region Intelligence
    void IntelligenceUpdate(float value, bool resetValue = true)
    {
        Intelligence = resetValue ? baseIntelligence + value : value;
        MaxMana = baseMana + Intelligence * INTELLIGENCE_MANA_SCALE;
        ManaRegeneration = baseManaRegeneration + Intelligence * INTELLIGENCE_MANAREGEN_SCALE;
    }
    public void AddIntelligence(float value)
    {
        Intelligence += value;
        IntelligenceUpdate(Intelligence, false);
    }
    #endregion
    #region Wisdom
    void WisdomUpdate(float value, bool resetValue = true)
    {
        Wisdom = resetValue ? baseWisdom + value : value;
        SpellDamage = baseSpellDamage + Wisdom * WISDOM_SPELLDAMAGE_SCALE;
    }
    public void AddWisdom(float value)
    {
        Wisdom += value;
        WisdomUpdate(Wisdom, false);
    }
    #endregion

    #endregion

    public void Attack(Entity target)
    {
        self.AttackAnimation();
        LastAttack = Time.time;
        if (UnityEngine.Random.value > HitChance) return;
        target.stats.Damage(UnityEngine.Random.value <= CritChance ? AttackDamage * CRIT_DAMAGE_MULTIPLIER : AttackDamage);
    }
    public void Damage(float amount)
    {
        amount *= 1 - PhysicalResistance;
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
            $"BaseMovementSpeed: {BaseMovementSpeed} \n" +
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
            $"PhysicalResistance: {PhysicalResistance} \n" +
            $"BleedResistance: {BleedResistance} \n" +
            $"PoisonResistance: {PoisonResistance} \n" +
            $"BurnResistance: {BurnResistance}";

        Debug.Log(statsMessage);
    }
}
