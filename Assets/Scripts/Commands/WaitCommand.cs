using UnityEngine;

public class WaitCommand : ICommand
{
    const float MAX_IDLE_DURATION = 12f;
    const float IDLE_DURATION_OFFSET = 3f;

    float lastIdle, idleTimer;

    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    public OrderType Type => OrderType.none;
    public GameObject WaypointObject { get; set; }

    public float? castTime => null;

    public WaitCommand(Entity entity)
    {
        this.Self = entity;

        if (entity.commands.Count == 0) BeginExecute();
    }

    public void BeginExecute()
    {
        lastIdle = Time.time;
        idleTimer = Random.value * MAX_IDLE_DURATION;

        BeingExecuted = true;
        Self.MoveAnimation(false);
    }

    public void OnFixedFrame()
    {
        if (Time.time > lastIdle + idleTimer + IDLE_DURATION_OFFSET)
        {
            lastIdle = Time.time;
            idleTimer = Random.value * MAX_IDLE_DURATION;
            Self.IdleTwo();
        }

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
