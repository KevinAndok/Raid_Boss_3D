using UnityEngine;
using System;

[Serializable]
public sealed class Buffs
{
    private Entity Self;

    public void Init(Entity self)
    {
        this.Self = self;
    }

}
