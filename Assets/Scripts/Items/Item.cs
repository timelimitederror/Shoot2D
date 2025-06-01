using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 道具基类
public abstract class Item : MonoBehaviour
{
    [SerializeField]
    protected int itemId = 0;
    public int ItemId { get { return itemId; } }
    [SerializeField]
    protected string itemName = "新道具";
    public string ItemName { get { return itemName; } }
    [SerializeField]
    protected string description = "道具描述";
    public string Description { get { return description; } }
    [SerializeField]
    protected Sprite icon; // 道具图片
    public Sprite Icon { get { return icon; } }
    [SerializeField]
    protected EquipmentSlotType slotType; // 道具类型
    public EquipmentSlotType SlotType { get { return slotType; } }
    [SerializeField]
    protected bool stackable = false; // 是否可以堆叠
    public bool Stackable { get { return stackable; } }
    protected int stackCount = 1; // 当前堆叠层数
    public int StackCount { get { return stackCount; } }
    [SerializeField]
    protected int maxStackCount = 1; // 最大堆叠层数

    protected PlayerMovement player; // 装备道具的玩家

    // 加载资源后，给这个道具设置信息
    public virtual void InitInfo(int itemId, string itemName, string description, Sprite icon, EquipmentSlotType slotType)
    {
        this.itemId = itemId;
        this.itemName = itemName;
        this.description = description;
        this.icon = icon;
        this.slotType = slotType;
    }

    // 装备后初始化道具（当道具被拾取时调用）
    public virtual void Initialize(PlayerMovement owner)
    {
        player = owner;
        transform.SetParent(owner.transform, false);
        //ApplyEffect();
    }

    // 道具被丢弃时调用
    public virtual void UnInitialize()
    {
        transform.SetParent(null, false);
    }

    // 应用道具效果
    public abstract void ApplyEffect();

    // 移除来自其他道具的加成效果 ->可以用道具加成其他道具 堆叠层数不受影响
    public virtual void Recovery()
    {
        // 子类可扩展
    }

    // 增加堆叠层数
    public virtual void AddStack(int amount = 1)
    {
        if (!stackable) return;

        stackCount = Mathf.Clamp(stackCount + amount, 1, maxStackCount);
        UpdateStackEffect();
    }

    // 更新堆叠效果（当堆叠层数变化时调用）
    protected virtual void UpdateStackEffect()
    {

    }
}
