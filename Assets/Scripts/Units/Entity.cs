using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Team { none, player, boss };

public class Entity : MonoBehaviour
{
    public bool DisplayStats = false;

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
    public StatusBar statusBar;

    public Stats stats;
    public Buffs buffs;
    public Debuffs debuffs;

    public bool IsDead { get => stats.Health <= 0; }
    public bool CanMove { get => !(debuffs.IsStunned || debuffs.IsFrozen); }
    public bool CanPerformActions { get => !(!CanMove || debuffs.IsTerrorized); }
    public bool CanCastSpells { get => !(!CanPerformActions || debuffs.IsSilenced); }

    protected virtual void Awake()
    {
        var parent = new GameObject(name);
        parent.transform.parent = transform.parent;
        transform.parent = parent.transform;

        EntityManager.AllEntities.Add(this);
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
        if (DisplayStats) stats.DisplayStats();

        WaitIfNoCommand();

        //Debug.Log(commands[0].GetType());

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
        if (commands.Count > 0)
        {
            if (commands[0].WaypointBeingDisplayed)
                PoolingSystem.GetPoolByName("Waypoint").ObjectPool.Release(commands[0].WaypointObject);
            commands.RemoveAt(0);
        }
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

    #region Animations
    private void DeathAnimation(bool state)
    {
        if (!animator) return;
        animator?.SetBool("IsDead", state);
    }
    public void MoveAnimation(bool state)
    {
        if (!animator) return;
        animator?.SetBool("IsWalking", state);
    }
    public void CastAnimation(bool state)
    {
        if (!animator) return;
        animator?.SetBool("Casting", state);
    }
    public void AttackAnimation()
    {
        if (!animator) return;
        animator.SetTrigger("Attack");
    }
    public void PowerUpAnimation()
    {
        if (!animator) return;
        animator.SetTrigger("PowerUp");
    }
    public void IdleTwo()
    {
        if (!animator) return;
        animator.SetTrigger("IdleTwo");
    }
    #endregion

    private void OnDeath()
    {
        DeathAnimation(true);
    }
    public void StopCurrentCommand()
    {
        if (commands[0].WaypointBeingDisplayed)
            PoolingSystem.GetPoolByName("Waypoint").ObjectPool.Release(commands[0].WaypointObject);
        commands[0].OnCancel();
    }
    public void StopAllCommands()
    {
        var pool = PoolingSystem.GetPoolByName("Waypoint");

        if (commands.Count == 0) return;

        foreach (var command in commands)
        {
            if (commands[0].WaypointObject && commands[0].WaypointObject.activeInHierarchy)
                pool.ObjectPool.Release(commands[0].WaypointObject);
            command.OnCancel();
        }

        commands.Clear();
    }
    public void DisplayCommands()
    {
        var pool = PoolingSystem.GetPoolByName("Waypoint");
        foreach (ICommand command in commands)
        {
            if (!command.WaypointBeingDisplayed)
            {
                command.DisplayCommand(pool);
            }
        }
    }
    public void HideCommands()
    {
        var pool = PoolingSystem.GetPoolByName("Waypoint");
        foreach (ICommand command in commands)
        {
            if (command.WaypointBeingDisplayed)
            {
                pool.ObjectPool.Release(command.WaypointObject);
                command.WaypointObject = null;
            }
        }
    }
}
