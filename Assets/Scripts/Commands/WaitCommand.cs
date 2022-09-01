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
    }

    public void OnComplete()
    {
        
    }

    public void OnExecute()
    {
        
    }
}
