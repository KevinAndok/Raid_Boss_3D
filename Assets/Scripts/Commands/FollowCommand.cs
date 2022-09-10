using UnityEngine;
using UnityEngine.AI;

public class FollowCommand : ICommand
{
    public Entity target;

    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    NavMeshAgent agent;
    float followDistance = 1.5f;

    public FollowCommand(Entity self, Entity target)
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
        if (Vector3.Distance(target.transform.position, Self.transform.position) <= followDistance)
        {
            if (agent.destination != Self.transform.position) agent.SetDestination(Self.transform.position);
            Self.MoveAnimation(false);
            return;
        }
        agent.SetDestination(target.transform.position);
        Self.MoveAnimation(true);
    }

    public void OnComplete()
    {
        //remove this from entity command list
        Self.NextCommand();
    }

    public void OnCancel()
    {
        Debug.LogWarning(new System.NotImplementedException());
    }
}
