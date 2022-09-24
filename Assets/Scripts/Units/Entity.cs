using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum Team { none, player, boss };

public class Entity : MonoBehaviour
{
    [SerializeField] private int level;
    public int Level { get => level; private set => level = value; }
    public Team team;
    public float entitySize = .5f;

    //public PlayerInput input; //for player
    public NavMeshAgent navigation;
    public Animator animator;
    public Transform model;
    public Collider unitCollider;
    public List<ICommand> commands = new List<ICommand>();

    public Stats stats;
    public Buffs buffs;
    public Debuffs debuffs;

    public bool IsDead { get => stats.Health <= 0; }
    public bool CanMove { get => !(debuffs.IsStunned || debuffs.IsFrozen); }
    public bool CanPerformActions { get => !(!CanMove || debuffs.IsTerrorized); }
    public bool CanCastSpells { get => !(!CanPerformActions || debuffs.IsSilenced); }

    protected virtual void Awake()
    {
        stats.OnDeath += OnDeath;
    }
    protected virtual void OnEnable()
    {
        stats.Init(this);
        debuffs.Init(this);
        buffs.Init(this);
    }
    protected virtual void FixedUpdate()
    {
        WaitIfNoCommand();

        Debug.Log(commands[0].GetType());

        if (!CheckIfCommandActive()) return;

        commands[0].OnFixedFrame();

        stats.RegenerateHealth();
        debuffs.ApplyDotDamage();
        debuffs.StatDebuffExpiration();

        //LookTowardsDirection();
    }

    protected virtual void Update()
    {
        
    }

    internal void NextCommand()
    {
        if (commands.Count > 0) commands.RemoveAt(0);
        if (commands.Count == 0)
        {
            commands.Add(new WaitCommand(this));
            return;
        }

        commands[0].BeginExecute();
    }

    bool CheckIfCommandActive() => commands.Count > 0 && commands[0].BeingExecuted;
    private void WaitIfNoCommand()
    {
        if (commands.Count == 0) commands.Add(new WaitCommand(this));
    }
    private void LookTowardsDirection()
    {
        if (navigation.destination != transform.position)
        {
            transform.LookAt(navigation.destination);
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

            model.localPosition = Vector3.down;
            model.rotation = Quaternion.Euler(Vector3.zero);
            model.localScale = Vector3.one;
        }
    }
    private void DeathAnimation(bool state) => animator.SetBool("IsDead", state);
    public void MoveAnimation(bool state) => animator.SetBool("IsWalking", state);
    public void CastAnimation(bool state) => animator.SetBool("Casting", state);
    public void AttackAnimation() => animator.SetTrigger("Attack");
    public void IdleTwo() => animator.SetTrigger("IdleTwo");
    private void OnDeath()
    {
        DeathAnimation(true);
    }
    public void StopCurrentCommand() => commands[0].OnCancel();
    public void StopAllCommands()
    {
        StopCurrentCommand();
        commands.Clear();
    }
}
