using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 主动道具父类
public abstract class ActiveItem : Item
{
    protected BoxCollider2D thisCollider;
    protected SpriteRenderer spriteRenderer;
    protected bool isUse = false;// 正在使用

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

        // 初始化参数
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
