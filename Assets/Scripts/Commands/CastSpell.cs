using System;

public abstract class CastSpell : ICommand
{
    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    public abstract ICommand GetCommand(Entity self);
    public abstract void BeginExecute();

    public abstract void OnCancel();

    public abstract void OnComplete();

    public abstract void OnExecute();
}
