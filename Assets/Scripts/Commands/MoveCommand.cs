using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public sealed class MoveCommand : ICommand
{
    public Vector3 position;

    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    public OrderType Type => OrderType.movement;
    public GameObject WaypointObject { get; set; }

    public float? castTime => null;

    public MoveCommand(Entity entity, Vector3 position)
    {
        this.Self = entity;
        this.position = position;

        if (entity.commands.Count > 0 && entity.commands[0].GetType() == typeof(WaitCommand)) entity.commands.RemoveAt(0);
        if (entity.commands.Count == 0) BeginExecute();
    }

    public void BeginExecute()
    {
        if (!Self.CanMove)
        {
            OnComplete();
            return;
        }
        Self.MoveAnimation(true);
        Self.GetComponent<NavMeshAgent>().SetDestination(position);
        BeingExecuted = true;
    }

    public void OnFixedFrame()
    {
        if (!Self.CanMove)
        {
            OnComplete();
            return;
        }

        if (new Vector2(Self.transform.position.x, Self.transform.position.z) == new Vector2(position.x, position.z))
        {
            OnComplete();
            return;
        }
        
        if (Self.commands.Count > 2 && Self.commands[1].GetType() == typeof(MoveCommand) &&
            Vector2.Distance(new Vector2(Self.transform.position.x, Self.transform.position.z), new Vector2(position.x, position.z)) < .5f)
        {
            OnComplete();
            return;
        }
    }

    public void OnComplete()
    {
        Self.MoveAnimation(false);
        //remove this from command command list
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
        waypoint.GetComponent<Waypoint>().Set(position, Self.entitySize, Type);
        WaypointObject = waypoint;
    }
}
