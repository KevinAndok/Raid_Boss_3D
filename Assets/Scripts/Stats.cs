using UnityEngine;

[System.Serializable]
public class Stats
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

    public void Init()
    {
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
    public void Attack() => LastAttack = Time.time;
    public void Damage(float amount) => Health = Mathf.Clamp(Health - amount, 0, MaxHealth);
    public void Heal(float amount) => Health = Mathf.Clamp(health + amount, 0, MaxHealth);
    public void RegenerateHealth() => Heal(HealthRegeneration * Time.fixedDeltaTime);
}
