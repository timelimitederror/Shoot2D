using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 消耗类回血道具，上限携带10个
public class HealingItem1 : ActiveItem
{
    public int healing = 30;
    public int originalHealing = 30;

    protected override void Awake()
    {
        base.Awake();
        stackable = true;
        maxStackCount = 10;
    }

    public override void Initialize(PlayerMovement owner)
    {
        base.Initialize(owner);
        healing = originalHealing;
    }

    public override void ApplyEffect()
    {
        if (stackCount > 0)
        {
            player.CurrentHealth += healing;
            stackCount--;
        }
        if (stackCount <= 0)
        {
            player.RemoveActiveItem(this);
        }
    }

    public override void Recovery()
    {
        healing = originalHealing;
    }
}
