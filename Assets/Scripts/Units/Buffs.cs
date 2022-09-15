using UnityEngine;
using System;

[Serializable]
public sealed class Buffs
{
    private Entity self;

    public void Init(Entity self)
    {
        this.self = self;
    }

}
