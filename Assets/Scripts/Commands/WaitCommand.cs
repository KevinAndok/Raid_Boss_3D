using UnityEngine;

public class WaitCommand : ICommand
{
    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

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

    public void OnExecute()
    {
        if (Random.value < .001f) Self.IdleTwo();
    }

    public void OnComplete()
    {
        
    }
}
