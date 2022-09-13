public interface ICommand
{
    public abstract bool BeingExecuted { get; set; }
    public abstract Entity Self { get; set; }
    public abstract void BeginExecute();                //called when command starts
    public abstract void OnFixedFrame();                //called in fixed update
    public abstract void OnComplete();                  //called when command finishes executing
    public abstract void OnCancel();                    //called when command finishes executing
    public abstract void OnInterrupt();                 //called when command is interrupted by an enemy
    public virtual bool EntityWaiting(Entity entity)    //used in constructor
    {
        return entity.commands.Count == 0 || entity.commands[0].GetType() == typeof(WaitCommand);
    }
}
