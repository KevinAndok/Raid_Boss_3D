using UnityEngine;
using UnityEngine.AI;

public class MoveCommand : ICommand
{
    public Vector3 position;

    public bool BeingExecuted { get; set; }
    public Entity Entity { get; set; }

    public MoveCommand(Entity entity, Vector3 position)
    {
        this.Entity = entity;
        this.position = position;

        if (entity.commands.Count > 0 && entity.commands[0].GetType() == typeof(WaitCommand)) entity.commands.RemoveAt(0);
        if (entity.commands.Count == 0) BeginExecute();
    }

    public void BeginExecute()
    {
        Entity.GetComponent<NavMeshAgent>().SetDestination(position);
        BeingExecuted = true;
    }

    public void OnExecute()
    {
        if (new Vector2(Entity.transform.position.x, Entity.transform.position.z) == new Vector2(position.x, position.z)) 
        {
            OnComplete();
            return;
        }
    }

    public void OnComplete()
    {
        //remove this from entity command list
        Entity.NextCommand();
    }
}
