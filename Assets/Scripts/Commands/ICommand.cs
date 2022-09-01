public interface ICommand
{
    public abstract bool BeingExecuted { get; set; }
    public abstract Entity Entity { get; set; }
    public abstract void BeginExecute();                //called when command starts
    public abstract void OnExecute();                   //called in fixed update
    public abstract void OnComplete();                  //called when command finishes executing
    public virtual bool EntityWaiting(Entity entity)    //used in constructor
    {
        return entity.commands.Count == 0 || entity.commands[0].GetType() == typeof(WaitCommand);
    }
}
