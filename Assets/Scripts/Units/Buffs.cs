using UnityEngine;
using System;

[Serializable]
public sealed class Buffs
{
    [SerializeField] private float health;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float healthRegeneration;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackDamage;

    public float Health { get; private set; }
    public float MaxHealth { get; private set; }
    public float HealthRegeneration { get; private set; }
    public float MovementSpeed { get; private set; }
    public float BaseMovementSpeed { get; private set; }
    public float AttackSpeed { get; private set; }
    public float AttackRange { get; private set; }
    public float AttackDamage { get; private set; }
    public float LastAttack { get; private set; }

    Entity self;

    public event Action OnDeath;

    public void Init(Entity self)
    {
        this.self = self;

        Health = health;
        MaxHealth = health;
        HealthRegeneration = healthRegeneration;
        MovementSpeed = movementSpeed;
        BaseMovementSpeed = movementSpeed;
        AttackSpeed = attackSpeed;
        AttackRange = attackRange;
        AttackDamage = attackDamage;
        LastAttack = 0;
    }

    public void ApplySlow()
    {

    }
    public void Attack(Entity target)
    {
        self.AttackAnimation();
        LastAttack = Time.time;
        target.stats.Damage(AttackDamage);
    }
    public void Damage(float amount)
    {
        Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
        if (Health == 0) OnDeath?.Invoke();
    }
    public void Heal(float amount) => Health = Mathf.Clamp(health + amount, 0, MaxHealth);
    public void RegenerateHealth() => Heal(HealthRegeneration * Time.fixedDeltaTime);
}
