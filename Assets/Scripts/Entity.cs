using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

public class Entity : MonoBehaviour
{
    //public PlayerInput input; //for player
    public NavMeshAgent navigation;
    public List<ICommand> commands = new List<ICommand>();

    private void Start()
    {
        var position = new Vector3(Random.value * 10, transform.position.y, Random.value * 10);

        navigation.Raycast(new Vector3(Random.value * 10, transform.position.y, Random.value * 10), out var hit);

        commands.Add(new MoveCommand(this, hit.position));
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
