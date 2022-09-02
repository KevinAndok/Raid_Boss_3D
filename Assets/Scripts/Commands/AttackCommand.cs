using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class AttackCommand : ICommand
{
    public Entity target;
    public NavMeshAgent agent;

    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    public AttackCommand(Entity self, Entity target)
    {
        agent = self.GetComponent<NavMeshAgent>();

        this.Self = self;
        this.target = target;

        if (self.commands.Count > 0 && self.commands[0].GetType() == typeof(WaitCommand)) self.commands.RemoveAt(0);
        if (self.commands.Count == 0) BeginExecute();
    }
    
    public void BeginExecute()
    {
        agent.SetDestination(target.transform.position);
        BeingExecuted = true;
    }

    public void OnExecute()
    {
        if (Vector3.Distance(target.transform.position, Self.transform.position) > Self.stats.AttackRange)
        {
            agent.SetDestination(target.transform.position);
            Self.MoveAnimation(true);
            return;
        }

        Self.transform.LookAt(Self.navigation.destination);
        Self.transform.rotation = Quaternion.Euler(0, Self.transform.rotation.eulerAngles.y, 0);

        Self.MoveAnimation(false);

        if (agent.destination != Self.transform.position) agent.SetDestination(Self.transform.position);

        if (Time.time > Self.stats.LastAttack + (1 / Self.stats.AttackSpeed))
        {
            Self.stats.Attack(target);
            Debug.Log(target.stats.Health);
        }
    }

    public void OnComplete()
    {
        //remove this from entity command list
        Self.NextCommand();
    }
}
