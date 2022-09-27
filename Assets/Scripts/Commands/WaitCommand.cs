using UnityEngine;

public class WaitCommand : ICommand
{
    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    public OrderType Type => OrderType.none;
    public GameObject WaypointObject { get; set; }

    public WaitCommand(Entity entity)
    {
        this.Self = entity;

        if (entity.commands.Count == 0) BeginExecute();
    }

    public void BeginExecute()
    {
        BeingExecuted = true;
        Self.MoveAnimation(false);
    }

    public void OnFixedFrame()
    {
        if (Random.value < .001f) Self.IdleTwo();

        //collision
    }

    public void OnComplete()
    {
        //this command never completes complete
        return;
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
        return;
    }
}
