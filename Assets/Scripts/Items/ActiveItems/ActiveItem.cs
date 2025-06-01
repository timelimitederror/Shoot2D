using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �������߸���
public abstract class ActiveItem : Item
{
    protected BoxCollider2D thisCollider;
    protected SpriteRenderer spriteRenderer;
    protected bool isUse = false;// ����ʹ��

    protected virtual void Awake()
    {
        thisCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Initialize(PlayerMovement owner)
    {
        base.Initialize(owner);
        player = owner;
        isUse = true;
        thisCollider.enabled = false;
        spriteRenderer.enabled = false;

        // ��ʼ������
        stackCount = 1;
    }

    public override void UnInitialize()
    {
        base.UnInitialize();
        isUse = false;
        thisCollider.enabled = true;
        spriteRenderer.enabled = true;
    }
}
