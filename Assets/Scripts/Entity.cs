using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public enum Team { none, player, boss };

public class Entity : MonoBehaviour
{
    //public PlayerInput input; //for player
    public NavMeshAgent navigation;
    public List<ICommand> commands = new List<ICommand>();

    public Team team;
    public GameObject selectionCircle;

    public Stats stats;

    private void Awake()
    {
        stats.Init();
    }

    private void FixedUpdate()
    {
        WaitIfNoCommand();

        Debug.Log(commands[0].GetType());

        if (!CheckIfCommandActive()) return;

        commands[0].OnExecute();
    }

    internal void NextCommand()
    {
        commands.RemoveAt(0);
        if (commands.Count == 0)
        {
            //wait command
            return;
        }

        commands[0].BeginExecute();
    }

    bool CheckIfCommandActive() => commands.Count > 0 && commands[0].BeingExecuted;
    private void WaitIfNoCommand()
    {
        if (commands.Count == 0) commands.Add(new WaitCommand(this));
    }
}
