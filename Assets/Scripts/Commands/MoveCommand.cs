using UnityEngine;
using UnityEngine.AI;

public class MoveCommand : ICommand
{
    public Vector3 position;

    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    public MoveCommand(Entity entity, Vector3 position)
    {
        this.Self = entity;
        this.position = position;

        if (entity.commands.Count > 0 && entity.commands[0].GetType() == typeof(WaitCommand)) entity.commands.RemoveAt(0);
        if (entity.commands.Count == 0) BeginExecute();
    }

    public void BeginExecute()
    {
        Self.MoveAnimation(true);
        Self.GetComponent<NavMeshAgent>().SetDestination(position);
        BeingExecuted = true;
    }

    public void OnExecute()
    {
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
        //remove this from entity command list
        Self.NextCommand();
    }

    public void OnCancel()
    {
        Debug.LogWarning(new System.NotImplementedException());
    }
}
