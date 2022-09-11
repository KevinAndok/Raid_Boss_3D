using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class TestingSpell : ICommand //ID 0
{
    Vector3 Ground;
    float time = 2;

    public TestingSpell(Entity self, Vector3 ground)
    {
        Self = self;
        Ground = ground;

        if (self.commands.Count == 0) BeginExecute();
    }

    public bool BeingExecuted { get; set; }
    public Entity Self { get; set; }

    public void BeginExecute()
    {
        BeingExecuted = true;
        Self.CastAnimation(true);
        Debug.Log("Begun");
    }

    public void OnCancel()
    {
        Self.CastAnimation(false);
        Self.NextCommand();

        Debug.Log("Cancelled");
    }

    public void OnComplete()
    {
        Self.CastAnimation(false);
        Self.NextCommand();
        Debug.Log("Completed");
    }

    public void OnExecute()
    {
        time -= Time.fixedDeltaTime;
        Debug.Log(time);
        if (time <= 0)
        {
            Self.transform.position = Ground;
            OnComplete();
        }
    }
}
