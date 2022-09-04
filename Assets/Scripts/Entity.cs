using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum Team { none, player, boss };

public class Entity : MonoBehaviour
{
    //public PlayerInput input; //for player
    public NavMeshAgent navigation;
    public Animator animator;
    public Transform model;
    public List<ICommand> commands = new List<ICommand>();

    public Team team;
    public GameObject selectionCircle;

    public Stats stats;

    private void Awake()
    {
        stats.OnDeath += OnDeath;
    }
    private void OnEnable()
    {
        stats.Init(this);
    }
    private void FixedUpdate()
    {
        WaitIfNoCommand();

        Debug.Log(commands[0].GetType());

        if (!CheckIfCommandActive()) return;

        commands[0].OnExecute();

        stats.RegenerateHealth();

        //LookTowardsDirection();
    }

    internal void NextCommand()
    {
        commands.RemoveAt(0);
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
    public void AttackAnimation() => animator.SetTrigger("Attack");
    public void IdleTwo() => animator.SetTrigger("IdleTwo");
    private void OnDeath()
    {
        DeathAnimation(true);
    }
}
