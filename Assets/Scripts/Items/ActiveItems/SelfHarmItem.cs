using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ��Ѫ���ܣ����ߣ����ɶѵ�
public class SelfHarmItem : ActiveItem
{
    public int healing = 30;
    public int originalHealing = 30;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void Initialize(PlayerMovement owner)
    {
        base.Initialize(owner);
        healing = originalHealing;
    }

    public override void ApplyEffect()
    {
        player.CurrentHealth -= healing;
    }

    public override void Recovery()
    {
        healing = originalHealing;
    }
}
