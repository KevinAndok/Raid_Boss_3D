using UnityEngine;

public class TestingSpell : CastSpell
{
    Vector3 Ground;
    float time = 2;

    public TestingSpell(Entity self, Vector3 ground)
    {
        Self = self;
        Ground = ground;
    }

    public override void BeginExecute()
    {
        BeingExecuted = true;
        Self.CastAnimation(true);
    }

    public override ICommand GetCommand(Entity self)
    {
        if (!PlayerCommander.isPointingAtGround) return null;
        return new TestingSpell(self, PlayerCommander.pointingAtGround);
    }

    public override void OnCancel()
    {
        Self.CastAnimation(false);
    }

    public override void OnComplete()
    {
        Self.CastAnimation(false);
        Self.transform.position = Ground;
    }

    public override void OnExecute()
    {
        time -= Time.fixedDeltaTime;
        if (time <= 0) OnComplete();
    }
}
