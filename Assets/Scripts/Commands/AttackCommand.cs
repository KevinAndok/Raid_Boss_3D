using UnityEngine;

public sealed class AttackCommand : ICommand
{
    public Entity target;

    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    public OrderType Type => OrderType.offensive;
    public GameObject WaypointObject { get; set; }

    public AttackCommand(Entity self, Entity target)
    {
        this.Self = self;
        this.target = target;

        if (self.commands.Count > 0 && self.commands[0].GetType() == typeof(WaitCommand)) self.commands.RemoveAt(0);
        if (self.commands.Count == 0) BeginExecute();
    }
    
    public void BeginExecute()
    {
        if (!Self.CanPerformActions)
        {
            Self.NextCommand();
            return;
        }

        Self.navigation.SetDestination(target.transform.position);
        BeingExecuted = true;
    }

    public void OnFixedFrame()
    {
        if (!Self.CanPerformActions)
        {
            Self.StopAllCommands();
            Self.NextCommand();
            return;
        }

        if (Vector3.Distance(target.transform.position, Self.transform.position) > Self.stats.AttackRange + Self.entitySize + target.entitySize)
        {
            Self.navigation.SetDestination(target.transform.position);
            Self.MoveAnimation(true);
            return;
        }

        Self.transform.LookAt(Self.navigation.destination);
        Self.transform.rotation = Quaternion.Euler(0, Self.transform.rotation.eulerAngles.y, 0);

        Self.MoveAnimation(false);

        if (Self.navigation.destination != Self.transform.position) Self.navigation.SetDestination(Self.transform.position);

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

    public void OnCancel()
    {
        return;
    }

    public void OnInterrupt()
    {
        return;
    }

    public void DisplayCommand(Pool waypointPool)
    {
        var waypoint = waypointPool.ObjectPool.Get();
        waypoint.GetComponent<Waypoint>().Set(target.transform, target.entitySize, Type);
        WaypointObject = waypoint;
    }
}
