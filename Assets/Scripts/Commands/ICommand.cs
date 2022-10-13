using UnityEngine;

public enum OrderType { none = -1, movement, offensive, defensive, utility }
public interface ICommand
{
    public abstract GameObject WaypointObject { get; set; }
    public virtual bool WaypointBeingDisplayed { get => WaypointObject != null && WaypointObject.activeInHierarchy; }
    public abstract float? castTime { get; }
    public abstract OrderType Type { get; }
    public abstract bool BeingExecuted { get; set; }
    public abstract Entity Self { get; set; }
    public abstract void BeginExecute();                            //called when command starts
    public abstract void OnFixedFrame();                            //called in fixed update
    public abstract void OnComplete();                              //called when command finishes executing
    public abstract void OnCancel();                                //called when command stops executing
    public abstract void OnInterrupt();                             //called when command is interrupted by an enemy
    public abstract void DisplayCommand(Pool waypointPool);         //called when a unit is selected or finished a command
    public virtual bool EntityWaiting(Entity entity)                //used in constructor
    {
        return entity.commands.Count == 0 || entity.commands[0].GetType() == typeof(WaitCommand);
    }
}
